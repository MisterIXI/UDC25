using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LevelOrchestrator : MonoBehaviour
{
    public static Action OnCurrentNodeChanged;
    [field: SerializeField] public NodeContainer StartContainer { get; private set; }
    public static LevelOrchestrator Instance { get; private set; }
    public AnchorList CurrentAnchorList { get; private set; }
    public NodeContainer CurrentContainer { get; private set; }
    public LevelNodeData CurrentNode { get; private set; }
    private HashSet<FrustumCulling> subscribedCullingObjects = new HashSet<FrustumCulling>();
    private Dictionary<FrustumCulling, string> spawnedObjects = new Dictionary<FrustumCulling, string>();
    private HashSet<string> GUIDsWaitingForDespawn = new HashSet<string>();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (StartContainer == null)
        {
            Debug.LogError("StartContainer not found. Levelinformation missing, self destructing");
            Destroy(gameObject);
            return;
        }
        CurrentAnchorList = FindObjectOfType<AnchorList>();
        if (CurrentAnchorList == null)
            Debug.LogError("AnchorList not found, please add the entrypoint to the scene");
    }
    private void Start()
    {
        PrefabPool.DiscoverAndBuildPrefabs(StartContainer);
        SetAnchorListInitial(CurrentAnchorList);
        UpdateSubscriptions();
        FlagManager.OnFlagSet += OnFlagsChanged;

    }
    private void SetAnchorListInitial(AnchorList anchorList)
    {
        CurrentAnchorList = anchorList;
        CurrentNode = CurrentAnchorList.LevelNodeData;
        CurrentContainer = CurrentNode.NodeContainer;
        UpdateSubscriptions();
    }
    public static void SetNewAnchorList(AnchorList anchorList, FrustumCulling frustumCulling)
    {
        if (Instance.CurrentAnchorList != anchorList)
            Instance.SetNewAnchorListPrivate(anchorList, frustumCulling);
    }
    private void SetNewAnchorListPrivate(AnchorList anchorList, FrustumCulling frustumCulling)
    {
        // Debug.Log($"SetNewAnchorListPrivate {anchorList.name} from {frustumCulling.name}");
        string guid = CurrentAnchorList.LevelNodeData.GUID;
        List<string> oldGUIDs = spawnedObjects.Values.Where(x => x != guid && x != anchorList.LevelNodeData.GUID).ToList();
        CurrentAnchorList = anchorList;
        CurrentNode = CurrentAnchorList.LevelNodeData;
        CurrentContainer = CurrentNode.NodeContainer;
        UnsubscribeFromAnchors();
        spawnedObjects.Add(frustumCulling, guid);
        UpdateSubscriptions();
        GUIDsWaitingForDespawn.UnionWith(oldGUIDs);
        CheckForNodeDespawn();
    }
    /// <summary>
    /// Discover all nodes from the currentNode and Link them in the cu
    /// </summary>
    private void UpdateSubscriptions()
    {
        SubscribeToAnchors(CurrentAnchorList);
        foreach (var frustumCulling in subscribedCullingObjects)
        {
            CheckNodeForSpawn(frustumCulling);
        }
    }
    private void OnFrustumCullingChange(FrustumCulling frustumCulling, bool visible)
    {
        CheckNodeForSpawn(frustumCulling);
        CheckForNodeDespawn();
    }
    private void OnFlagsChanged(string flagName, bool value)
    {
        foreach (var frustumCulling in subscribedCullingObjects)
        {
            CheckNodeForSpawn(frustumCulling);
        }
        CheckForNodeDespawn();
    }

    private void CheckNodeForSpawn(FrustumCulling frustumCulling)
    {
        NodeLinkData nextNodeLink = CurrentAnchorList.LevelNodeData.GetNodeLinkDataOfNextValidLevelNode(frustumCulling.name);
        LevelNodeData nextNode = nextNodeLink.TargetNodeGUID.ConvertGuidStringToBaseNode(nextNodeLink.NodeContainer) as LevelNodeData;
        if (nextNode == null)
            return;
        if (spawnedObjects.ContainsKey(frustumCulling))
        { // if object is spawned here
            if (spawnedObjects[frustumCulling] != nextNode.GUID)
            { // if the object is not the one that should be spawned here
                if (!frustumCulling.IsCurrentlyVisible)
                { // if is allowed to switch objects
                    PrefabPool.DespawnAnchorList(spawnedObjects[frustumCulling]);
                    string targetPortName = CurrentNode.GetNodeLinkDataOfNextValidLevelNode(frustumCulling.name).TargetPortName;
                    PrefabPool.SpawnAnchorListAtPosition(nextNode.GUID, targetPortName, frustumCulling.transform.position);
                    spawnedObjects[frustumCulling] = nextNode.GUID;
                }
            }
        }
        else
        { // currently no object spawned here
            if (!frustumCulling.IsCurrentlyVisible)
            { // if is allowed to spawn object
                string targetPortName = CurrentNode.GetNodeLinkDataOfNextValidLevelNode(frustumCulling.name).TargetPortName;
                PrefabPool.SpawnAnchorListAtPosition(nextNode.GUID, targetPortName, frustumCulling.transform.position);
                spawnedObjects.Add(frustumCulling, nextNode.GUID);
            }
        }
    }

    private void CheckForNodeDespawn()
    {
        List<string> guids = new List<string>();
        foreach (var guid in GUIDsWaitingForDespawn)
        {
            if (PrefabPool.TryDespawningAnchorList(guid))
                guids.Add(guid);
        }
        guids.ForEach(x => GUIDsWaitingForDespawn.Remove(x));
    }
    private void SubscribeToAnchors(AnchorList anchorList)
    {
        foreach (GeometryAnchor anchor in anchorList.anchors)
        {
            anchor.FrustumCulling.OnCameraFrustumStatusChangedWithSelf += OnFrustumCullingChange;
            subscribedCullingObjects.Add(anchor.FrustumCulling);
        }
    }

    private void UnsubscribeFromAnchors()
    {
        foreach (FrustumCulling cullingObj in subscribedCullingObjects)
        {
            cullingObj.OnCameraFrustumStatusChangedWithSelf -= OnFrustumCullingChange;
        }
        spawnedObjects.Clear();
        subscribedCullingObjects.Clear();
    }
}