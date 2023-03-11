using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PrefabPool : MonoBehaviour
{
    public static PrefabPool Instance { get; private set; }
    /// <summary>
    /// The prefab pool contains all spawned objects. The key is the GUID of the corresponding nodes. This ensures that duplicate objects on different Nodes are a seperate entry in the pool.
    /// </summary>
    private Dictionary<string, AnchorList> _prefabPool = new Dictionary<string, AnchorList>();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
    }
    public static AnchorList GetInstantiatedAnchorList(string guid)
    {
        if (Instance._prefabPool.ContainsKey(guid))
            return Instance._prefabPool[guid];
        return null;
    }
    public static AnchorList SpawnAnchorListAtPosition(string guid, string portName, Vector3 position)
    {
        var prefabPool = Instance._prefabPool;
        if (!prefabPool.ContainsKey(guid))
        {
            Debug.LogError("GUID not found in prefab pool");
            return null;
        }
        if (prefabPool[guid].gameObject.activeSelf)
        {
            Debug.LogWarning($"Tried spawning already active object: {guid} on port {portName}");
            return null;
        }
        AnchorList anchorList = prefabPool[guid];
        anchorList.transform.rotation = Quaternion.Euler(0, anchorList.LevelNodeData.YRotation, 0);
        Vector3 offset = anchorList.anchors.First(x => x.name == portName).transform.position - anchorList.transform.position;
        anchorList.transform.position = position - offset;
        anchorList.gameObject.SetActive(true);
        return anchorList;
    }
    public static bool TryDespawningAnchorList(string guid)
    {
        var prefabPool = Instance._prefabPool;
        if (!prefabPool.ContainsKey(guid))
        {
            Debug.LogError("GUID not found in prefab pool");
            return false;
        }
        if (!prefabPool[guid].gameObject.activeSelf)
        {
            Debug.LogWarning($"Tried despawning already inactive object: {guid}");
            return false;
        }
        if (!prefabPool[guid].anchors.Any(x => x.FrustumCulling.IsCurrentlyVisible))
        {// If none of the anchors are visible, despawn the object
            DespawnAnchorList(guid);
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void DespawnAnchorList(string guid)
    {
        var prefabPool = Instance._prefabPool;
        if (!prefabPool.ContainsKey(guid))
        {
            Debug.LogError("GUID not found in prefab pool");
            return;
        }
        Debug.Log($"Despawning {guid}");
        prefabPool[guid].gameObject.SetActive(false);
    }

    public static void DiscoverAndBuildPrefabs(NodeContainer nodeContainer)
    {
        Instance.DiscoverAndSpawnAllNodeContainerPrefabs(nodeContainer);
    }
    private void DiscoverAndSpawnAllNodeContainerPrefabs(NodeContainer nodeContainer)
    {
        HashSet<BaseNodeData> visitedNodes = new HashSet<BaseNodeData>();
        Queue<BaseNodeData> nodesToVisit = new Queue<BaseNodeData>();
        LevelNodeData EntryLevelNode = nodeContainer.GetEntryNode().TryGetConnectedNode();
        nodesToVisit.Enqueue(EntryLevelNode);
        while (nodesToVisit.Count > 0)
        {
            BaseNodeData currentNodeData = nodesToVisit.Dequeue();
            if (visitedNodes.Contains(currentNodeData))
                continue;
            visitedNodes.Add(currentNodeData);
            if (currentNodeData is LevelNodeData levelNodeData)
            {
                if (EntryLevelNode == currentNodeData)
                {
                    Debug.Log("Entry node found");
                    _prefabPool.Add(currentNodeData.GUID, LevelOrchestrator.Instance.CurrentAnchorList);
                    _prefabPool[currentNodeData.GUID].LevelNodeData = levelNodeData;
                }
                else if (!_prefabPool.ContainsKey(currentNodeData.GUID))
                {
                    var spawnedObject = Instantiate(levelNodeData.anchorList);
                    spawnedObject.gameObject.SetActive(false);
                    spawnedObject.LevelNodeData = levelNodeData;
                    _prefabPool.Add(levelNodeData.GUID, spawnedObject);
                }
                else
                {
                    Debug.LogError("Duplicate GUID detected");
                }
            }
            else if (currentNodeData is DecisionNodeData decisionNodeData)
            {
                // do nothing
            }
            else if (currentNodeData is LinkNodeData linkNodeData)
            {
                nodesToVisit.Enqueue(linkNodeData.linkedContainer.GetEntryNode());
            }
            List<BaseNodeData> connectedNodes = currentNodeData.GetConnectedNodes();
            foreach (BaseNodeData connectedNode in connectedNodes)
            {
                nodesToVisit.Enqueue(connectedNode);
            }
        }
    }

}