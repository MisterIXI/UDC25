using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class NodeExtensions
{
    public static LevelNodeData TryGetConnectedNode(this LinkNodeData linkNode)
    {
        BaseNodeData baseNode = linkNode.NodeContainer.GetConnectedNodes(linkNode).FirstOrDefault();
        Debug.Log("(First entry node call) TryGetConnectedNode: " + baseNode);
        if (baseNode == null || baseNode is not LevelNodeData levelNode)
        {
            Debug.LogError("No node entry found. Invalid nodecontainer!");
            return null;
        }
        return baseNode as LevelNodeData;
    }
    public static NodeLinkData GetNodeLinkDataByDecision(this DecisionNodeData decisionNode)
    {
        NodeContainer nodeContainer = decisionNode.NodeContainer;
        bool decision = FlagManager.GetFlag(decisionNode.flagName);
        var nodeLinks = decisionNode.GetOutGoingNodeLinks();
        if (decision)
        {
            return nodeLinks.FirstOrDefault(x => x.BasePortName == "True");
        }
        else
        {
            return nodeLinks.FirstOrDefault(x => x.BasePortName == "False");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelNode"></param>
    /// <param name="nodeContainer"></param>
    /// <returns>Connected nodes by portname</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Dictionary<string, LevelNodeData> GetCurrentlyValidConnectedNodes(this LevelNodeData levelNode)
    {
        NodeContainer nodeContainer = levelNode.NodeContainer;
        Dictionary<string, LevelNodeData> connectedNodes = new Dictionary<string, LevelNodeData>();
        // List<LevelNodeData> connectedNodes = new List<LevelNodeData>();
        List<NodeLinkData> nodeLinks = nodeContainer.NodeLinkData.Where(x => x.BaseNodeGUID == levelNode.GUID).ToList();
        foreach (NodeLinkData nodeLink in nodeLinks)
        {
            var nextNodeGUID = levelNode.GetNodeLinkDataOfNextValidLevelNode(nodeLink.BasePortName);
            var nextNode = nodeContainer.GetNodeDataByGUID(nextNodeGUID.TargetNodeGUID) as LevelNodeData;
            connectedNodes.Add(nodeLink.BasePortName, nextNode);
        }
        return connectedNodes;
    }


    public static NodeLinkData GetNodeLinkDataOfNextValidLevelNode(this LevelNodeData levelNode, String startPortName)
    {
        // Debug.Log($"GetNodeLinkDataOfNextValidLevelNode: {levelNode.GUID} {startPortName}");
        NodeContainer nodeContainer = levelNode.NodeContainer;
        var nodeLink = nodeContainer.NodeLinkData.FirstOrDefault(x => x.BaseNodeGUID == levelNode.GUID && x.BasePortName == startPortName);
        if (nodeLink == null)
            return null;
        BaseNodeData currentNode = nodeContainer.GetNodeDataByGUID(nodeLink.TargetNodeGUID);
        string targetPortName = nodeLink.TargetPortName;
        while (currentNode is not LevelNodeData)
        {
            if (currentNode is DecisionNodeData decisionNode)
            {
                nodeLink = decisionNode.GetNodeLinkDataByDecision();
                targetPortName = nodeLink.TargetPortName;
                currentNode = nodeContainer.GetNodeDataByGUID(nodeLink.TargetNodeGUID);
            }
            else if (currentNode is LinkNodeData linkNode)
            {
                nodeContainer = linkNode.linkedContainer;
                linkNode = nodeContainer.GetEntryNode();
                nodeLink = linkNode.GetSingleNodeLink();
                currentNode = linkNode.TryGetConnectedNode();
            }
        }
        return nodeLink;
    }
    public static List<NodeLinkData> GetOutGoingNodeLinks(this BaseNodeData node)
    {
        return node.NodeContainer.NodeLinkData.Where(x => x.BaseNodeGUID == node.GUID).ToList();
    }

    public static NodeLinkData GetSingleNodeLink(this LinkNodeData linkNode)
    {
        NodeLinkData nodeLink = linkNode.GetOutGoingNodeLinks().FirstOrDefault();
        if (nodeLink == null)
            nodeLink = linkNode.GetIncomingNodeLinks().FirstOrDefault();
        if (nodeLink == null)
        {
            Debug.LogError("No node link found for link node: " + linkNode.GUID);
            return null;
        }
        return nodeLink;
    }

    public static List<NodeLinkData> GetIncomingNodeLinks(this BaseNodeData node)
    {
        return node.NodeContainer.NodeLinkData.Where(x => x.TargetNodeGUID == node.GUID).ToList();
    }

    public static List<BaseNodeData> GetConnectedNodes(this BaseNodeData baseNodeData)
    {
        return baseNodeData.NodeContainer.GetConnectedNodes(baseNodeData);
    }

}

