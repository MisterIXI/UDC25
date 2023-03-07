using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LevelOrchestrator : MonoBehaviour
{
    [field: SerializeField] public NodeContainer StartContainer { get; private set; }
    public static LevelOrchestrator Instance { get; private set; }
    public AnchorList CurrentAnchorList { get; private set; }
    public NodeContainer CurrentContainer { get; private set; }
    private LevelNodeData CurrentNode;
    private HashSet<FrustumCulling> subscribedCullingObjects = new HashSet<FrustumCulling>();
    private Dictionary<FrustumCulling, string> cullingToGUID = new Dictionary<FrustumCulling, string>();
    private string previousNodeGUID;
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
        CurrentNode = StartContainer.GetEntryNode().TryGetConnectedNode(StartContainer);
        CurrentContainer = CurrentNode.NodeContainer;
    }
    private void Start()
    {
        UpdateSubscriptions();
        FlagManager.OnFlagSet += OnFlagsChanged;
    }
    public static void SetNewAnchorList(AnchorList anchorList, FrustumCulling frustumCulling)
    {
        if (Instance.CurrentAnchorList != anchorList)
            Instance.SetNewAnchorListPrivate(anchorList, frustumCulling);
    }
    private void SetNewAnchorListPrivate(AnchorList anchorList, FrustumCulling frustumCulling)
    {
        Debug.Log($"SetNewAnchorListPrivate {anchorList.name} from {frustumCulling.name}");
        string guid = CurrentAnchorList.LevelNodeData.GUID;
        CurrentAnchorList = anchorList;
        CurrentNode = CurrentAnchorList.LevelNodeData;
        CurrentContainer = CurrentNode.NodeContainer;
        UpdateSubscriptions();
        cullingToGUID.Add(frustumCulling, guid);
        if (!String.IsNullOrEmpty(previousNodeGUID))
        {
            PrefabPool.DespawnAnchorList(previousNodeGUID);
        }
        previousNodeGUID = guid;
    }
    /// <summary>
    /// Discover all nodes from the currentNode and Link them in the cu
    /// </summary>
    private void UpdateSubscriptions()
    {
        UnsubscribeFromAnchors();
        SubscribeToAnchors(CurrentAnchorList);
        foreach (var frustumCulling in subscribedCullingObjects)
        {
            CheckNodeForSpawn(frustumCulling);
        }
    }
    private void OnFrustumCullingChange(FrustumCulling frustumCulling, bool visible)
    {
        CheckNodeForSpawn(frustumCulling);
    }
    private void OnFlagsChanged(string flagName, bool value)
    {
        foreach (var frustumCulling in subscribedCullingObjects)
        {
            CheckNodeForSpawn(frustumCulling);
        }
    }

    private void CheckNodeForSpawn(FrustumCulling frustumCulling)
    {
        LevelNodeData nextNode = CurrentAnchorList.LevelNodeData.GetNextLevelNodeFromPortName(frustumCulling.name);
        if (nextNode == null)
            return;
        if (cullingToGUID.ContainsKey(frustumCulling))
        { // if object is spawned here
            if (cullingToGUID[frustumCulling] != nextNode.GUID)
            { // if the object is not the one that should be spawned here
                if (!frustumCulling.IsCurrentlyVisible)
                { // if is allowed to switch objects
                    Debug.Log($"11Spawn {nextNode.GUID} at {frustumCulling.name} and dictentry: {cullingToGUID[frustumCulling]}");
                    PrefabPool.DespawnAnchorList(cullingToGUID[frustumCulling]);
                    string targetPortName = CurrentNode.GetOutGoingNodeLinks().First(x => x.BasePortName == frustumCulling.name).TargetPortName;
                    PrefabPool.SpawnAnchorListAtPosition(nextNode.GUID, targetPortName, frustumCulling.transform.position);
                    cullingToGUID[frustumCulling] = nextNode.GUID;
                }
            }
        }
        else
        { // currently no object spawned here
            if (!frustumCulling.IsCurrentlyVisible)
            { // if is allowed to spawn object
                Debug.Log($"22Spawn {nextNode.GUID} at {frustumCulling.name}");
                string targetPortName = CurrentNode.GetOutGoingNodeLinks().First(x => x.BasePortName == frustumCulling.name).TargetPortName;
                PrefabPool.SpawnAnchorListAtPosition(nextNode.GUID, targetPortName, frustumCulling.transform.position);
                cullingToGUID.Add(frustumCulling, nextNode.GUID);
            }
        }
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
        cullingToGUID.Clear();
        subscribedCullingObjects.Clear();
    }
}