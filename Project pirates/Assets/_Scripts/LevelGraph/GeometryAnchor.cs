using System.Collections.Generic;
using UnityEngine;
public class GeometryAnchor : MonoBehaviour
{
    public string AnchorName => name;
    [field: SerializeField] public Bounds AnchorBounds { get; private set; }
}