using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
static class VariousExtensions
{
    public static BaseNodeData ConvertGuidStringToBaseNode(this String guid, NodeContainer nodeContainer)
    {
        return nodeContainer.GetNodeDataByGUID(guid);
    }
}