using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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
        DiscoverAndSpawnAllNodeContainerPrefabs();
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
        Vector3 offset = anchorList.anchors.First(x => x.name == portName).transform.localPosition;
        anchorList.transform.position = position - offset;
        anchorList.gameObject.SetActive(true);
        return anchorList;
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

    private void DiscoverAndSpawnAllNodeContainerPrefabs()
    {
        HashSet<BaseNodeData> visitedNodes = new HashSet<BaseNodeData>();
        Queue<BaseNodeData> nodesToVisit = new Queue<BaseNodeData>();
        nodesToVisit.Enqueue(LevelOrchestrator.Instance.StartContainer.GetEntryNode());
        LevelNodeData EntryLevelNode = LevelOrchestrator.Instance.StartContainer.GetEntryNode().TryGetConnectedNode(LevelOrchestrator.Instance.StartContainer);
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
            List<BaseNodeData> connectedNodes = currentNodeData.GetConnectedNodes();
            foreach (BaseNodeData connectedNode in connectedNodes)
            {
                nodesToVisit.Enqueue(connectedNode);
            }
        }
    }

}