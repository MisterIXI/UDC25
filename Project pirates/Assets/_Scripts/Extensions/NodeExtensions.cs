using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public static class NodeExtensions
{
    public static LevelNodeData TryGetConnectedNode(this LinkNodeData linkNode, NodeContainer nodeContainer)
    {
        BaseNodeData baseNode = nodeContainer.GetConnectedNodes(linkNode).FirstOrDefault();
        Debug.Log("(First entry node call) TryGetConnectedNode: " + baseNode);
        if (baseNode != null && baseNode is LevelNodeData levelNode)
        {
            return levelNode;
        }
        return null;
    }
    public static BaseNodeData GetNodeDataByDecision(this DecisionNodeData decisionNode, NodeContainer nodeContainer)
    {
        bool decision = FlagManager.GetFlag(decisionNode.flagName);
        var nodeLinks = decisionNode.GetOutGoingNodeLinks();
        if (decision)
        {
            return nodeContainer.GetNodeDataByGUID(nodeLinks.FirstOrDefault(x => x.BasePortName == "True").TargetNodeGUID);
        }
        else
        {
            return nodeContainer.GetNodeDataByGUID(nodeLinks.FirstOrDefault(x => x.BasePortName == "False").TargetNodeGUID);
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
            var nextNode = levelNode.GetNextLevelNodeFromPortName(nodeLink.BasePortName);
            connectedNodes.Add(nodeLink.BasePortName, nextNode);
        }
        return connectedNodes;
    }

    public static LevelNodeData GetNextLevelNodeFromPortName(this LevelNodeData levelNode, string portName)
    {
        NodeContainer nodeContainer = levelNode.NodeContainer;
        NodeLinkData nodeLink = nodeContainer.NodeLinkData.FirstOrDefault(x => x.BaseNodeGUID == levelNode.GUID && x.BasePortName == portName);
        if (nodeLink == null)
            return null;
        BaseNodeData currentNode = nodeContainer.GetNodeDataByGUID(nodeLink.TargetNodeGUID);
        while (currentNode is not LevelNodeData)
        {
            if (currentNode is DecisionNodeData decisionNode)
            {
                currentNode = decisionNode.GetNodeDataByDecision(nodeContainer);
            }
            else if (currentNode is LinkNodeData linkNode)
            {
                throw new NotImplementedException();
            }
        }
        return (LevelNodeData)currentNode;
    }
    public static List<BaseNodeData> GetConnectedNodes(this BaseNodeData baseNodeData)
    {
        return baseNodeData.NodeContainer.GetConnectedNodes(baseNodeData);
    }
    public static List<NodeLinkData> GetOutGoingNodeLinks(this BaseNodeData node)
    {

        return node.NodeContainer.NodeLinkData.Where(x => x.BaseNodeGUID == node.GUID).ToList();
    }
}

