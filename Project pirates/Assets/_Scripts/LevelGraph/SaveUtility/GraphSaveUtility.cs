using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class GraphSaveUtility
{
    private LevelGraphView _targetGraphView;
    public static GraphSaveUtility GetInstance(LevelGraphView graphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = graphView
        };
    }
    public void SaveGraph(NodeContainer container)
    {
        var NodeLinks = new List<NodeLinkData>();
        var levelNodeDatas = new List<LevelNodeData>();
        var decisionNodeDatas = new List<DecisionNodeData>();
        var linkNodeDatas = new List<LinkNodeData>();
        foreach (BaseNode node in _targetGraphView.nodes)
        {
            if (node is LevelNode)
            {
                var levelNode = node as LevelNode;
                levelNodeDatas.Add(new LevelNodeData
                {
                    GUID = levelNode.GUID,
                    DisplayName = levelNode.title,
                    position = levelNode.GetPosition(),
                    anchorList = levelNode.anchorList,
                    NodeContainer = container
                });
            }
            else if (node is DecisionNode)
            {
                var decisionNode = node as DecisionNode;
                decisionNodeDatas.Add(new DecisionNodeData
                {
                    GUID = decisionNode.GUID,
                    DisplayName = decisionNode.title,
                    position = decisionNode.GetPosition(),
                    flagName = decisionNode.flagName,
                    NodeContainer = container
                });
            }
            else if (node is LinkNode)
            {
                var linkNode = node as LinkNode;
                linkNodeDatas.Add(new LinkNodeData
                {
                    GUID = linkNode.GUID,
                    DisplayName = linkNode.title,
                    position = linkNode.GetPosition(),
                    container = linkNode.container,
                    IsEntryPoint = linkNode.IsEntryPoint,
                    NodeContainer = container
                });
            }
            foreach (var port in node.outputContainer.Children())
            {
                var outputPort = port as Port;
                foreach (Edge edge in outputPort.connections)
                {
                    NodeLinks.Add(new NodeLinkData
                    {
                        BaseNodeGUID = node.GUID,
                        BasePortName = outputPort.portName,
                        TargetNodeGUID = ((BaseNode)edge.input.node).GUID,
                        TargetPortName = ((Port)edge.input).portName
                    });
                }
            }
        }
        container.levelNodeData = levelNodeDatas;
        container.decisionNodeData = decisionNodeDatas;
        container.linkNodeData = linkNodeDatas;
        container.NodeLinkData = NodeLinks;
    }

    public void LoadGraph(NodeContainer container)
    {
        var levelNodeDatas = container.levelNodeData;
        var decisionNodeDatas = container.decisionNodeData;
        var linkNodeDatas = container.linkNodeData;
        var nodeLinkDatas = container.NodeLinkData;
        Dictionary<string, BaseNode> nodeDict = new Dictionary<string, BaseNode>();
        foreach (LevelNodeData nodeData in levelNodeDatas)
        {
            var levelNode = _targetGraphView.CreateAndAddLevelNode(nodeData.DisplayName, nodeData.GUID, nodeData.position, nodeData.anchorList);
            nodeDict.Add(nodeData.GUID, levelNode);
        }
        foreach (DecisionNodeData nodeData in decisionNodeDatas)
        {
            var decisionNode = _targetGraphView.CreateAndAddDecisionNode(nodeData.DisplayName, nodeData.GUID, nodeData.position, nodeData.flagName);
            nodeDict.Add(nodeData.GUID, decisionNode);
        }
        foreach (LinkNodeData nodeData in linkNodeDatas)
        {
            var linkNode = _targetGraphView.CreateAndAddLinkNode(nodeData.DisplayName, nodeData.GUID, nodeData.position, nodeData.container, nodeData.IsEntryPoint);
            nodeDict.Add(nodeData.GUID, linkNode);
        }
        foreach (BaseNode node in nodeDict.Values)
        {
            List<NodeLinkData> nodeLinkData = nodeLinkDatas.Where(x => x.BaseNodeGUID == node.GUID).ToList();
            foreach (var linkData in nodeLinkData)
            {
                Port outputPort = node.outputContainer.Children().First(x => x is Port && ((Port)x).portName == linkData.BasePortName) as Port;
                Port inputPort = nodeDict[linkData.TargetNodeGUID].inputContainer.Children().First(x => x is Port && ((Port)x).portName == linkData.TargetPortName) as Port;
                var tempEdge = new Edge
                {
                    output = outputPort,
                    input = inputPort
                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                _targetGraphView.Add(tempEdge);
            }
        }
        foreach (BaseNode node in _targetGraphView.nodes.ToList())
        {
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}