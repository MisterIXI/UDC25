using System.Collections.Generic;
using UnityEngine;
public class AnchorList : MonoBehaviour
{
    [field: SerializeField] public string GUID;
    [field: SerializeField] public List<GeometryAnchor> anchors { get; private set; } = new List<GeometryAnchor>();

}