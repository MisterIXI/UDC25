using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LinkNodeData : BaseNodeData
{
    public NodeContainer linkedContainer;
    public bool IsEntryPoint;
}