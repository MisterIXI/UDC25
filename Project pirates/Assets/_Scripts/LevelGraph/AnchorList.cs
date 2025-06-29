using System.Collections.Generic;
using UnityEngine;
public class AnchorList : MonoBehaviour
{
    [field: SerializeField] public List<GeometryAnchor> anchors { get; private set; } = new List<GeometryAnchor>();
    public LevelNodeData LevelNodeData;
    private void Start()
    {
        foreach (var anchor in anchors)
        {
            anchor.AnchorList = this;
        }
    }
}