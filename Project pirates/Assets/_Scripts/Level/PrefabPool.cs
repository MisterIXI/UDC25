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

    public static AnchorList SpawnAnchorListAtPosition(string guid, Vector3 position)
    {
        var prefabPool = Instance._prefabPool;
        if (!prefabPool.ContainsKey(guid))
        {
            Debug.LogError("GUID not found in prefab pool");
            return null;
        }
        if (prefabPool[guid].gameObject.activeSelf)
        {
            Debug.LogWarning("Tried spawning already active object");
            return null;
        }
        AnchorList anchorList = prefabPool[guid];
        anchorList.transform.position = position;
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
        prefabPool[guid].gameObject.SetActive(false);
    }

    private void DiscoverAndSpawnAllNodeContainerPrefabs()
    {
        HashSet<BaseNodeData> visitedNodes = new HashSet<BaseNodeData>();
        Queue<BaseNodeData> nodesToVisit = new Queue<BaseNodeData>();
        nodesToVisit.Enqueue(LevelOrchestrator.Instance.StartContainer.GetEntryNode());
        while (nodesToVisit.Count > 0)
        {
            BaseNodeData currentNodeData = nodesToVisit.Dequeue();
            if (visitedNodes.Contains(currentNodeData))
                continue;
            visitedNodes.Add(currentNodeData);
            if (currentNodeData is LevelNodeData)
            {
                if (!_prefabPool.ContainsKey(currentNodeData.GUID))
                {
                    LevelNodeData levelNodeData = currentNodeData as LevelNodeData;
                    var spawnedObject = Instantiate(levelNodeData.anchorList);
                    spawnedObject.gameObject.SetActive(false);
                    _prefabPool.Add(levelNodeData.GUID, spawnedObject);
                }
                else
                {
                    Debug.LogError("Duplicate GUID detected");
                }
            }
        }
    }

}