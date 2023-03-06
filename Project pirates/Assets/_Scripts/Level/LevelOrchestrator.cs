using System;
using System.Collections.Generic;
using UnityEngine;
public class LevelOrchestrator : MonoBehaviour
{
    public AnchorList CurrentAnchorList { get; private set; }
    [field: SerializeField] public NodeContainer StartContainer { get; private set; }
    public NodeContainer CurrentContainer { get; private set; }
    public List<AnchorList> AdjacentAnchors { get; private set; }
    public static LevelOrchestrator Instance { get; private set; }
    private HashSet<FrustumCulling> subscribedCullingObjects = new HashSet<FrustumCulling>();
    // private Dictionary<
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CurrentAnchorList = FindObjectOfType<AnchorList>();
        if (StartContainer == null)
        {
            Debug.LogError("StartContainer not found. Levelinformation missing, self destructing");
            Destroy(gameObject);
            return;
        }
        if (CurrentAnchorList == null)
            Debug.LogError("AnchorList not found, please add the entrypoint to the scene");
        // CurrentContainer = 
    }
    private void Start()
    {

    }


}