using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// This class contains extension methods for the NodeContainer class.
/// </summary>
public static class NodeContainerExtensions
{
    public static LinkNodeData GetEntryNode(this NodeContainer nodeContainer)
    {
        return nodeContainer.linkNodeData.FirstOrDefault(x => x.IsEntryPoint);
    }

    public static List<BaseNodeData> GetConnectedNodes(this NodeContainer nodeContainer, BaseNodeData node)
    {
        List<BaseNodeData> connectedNodes = new List<BaseNodeData>();
        List<NodeLinkData> nodeLinks = nodeContainer.NodeLinkData.Where(x => x.BaseNodeGUID == node.GUID).ToList();
        foreach (NodeLinkData nodeLink in nodeLinks)
        {
            connectedNodes.Add(nodeContainer.GetNodeDataByGUID(nodeLink.TargetNodeGUID));
        }
        return connectedNodes;
    }

  


    public static BaseNodeData GetNodeDataByGUID(this NodeContainer nodeContainer, string guid)
    {
        BaseNodeData nodeData = nodeContainer.levelNodeData.FirstOrDefault(x => x.GUID == guid);
        if (nodeData != null)
        {
            return nodeData;
        }
        nodeData = nodeContainer.decisionNodeData.FirstOrDefault(x => x.GUID == guid);
        if (nodeData != null)
        {
            return nodeData;
        }
        nodeData = nodeContainer.linkNodeData.FirstOrDefault(x => x.GUID == guid);
        if (nodeData != null)
        {
            return nodeData;
        }
        return null;
    }
}