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
    public void SaveGraph(LevelNodeContainer container)
    {
        var levelNodeContainer = ScriptableObject.CreateInstance<LevelNodeContainer>();
        var levelNodeDatas = new List<LevelNodeData>();
        var levelND_Dict = new Dictionary<string, List<LevelNodeLinkData>>();
        foreach (var node in _targetGraphView.nodes)
        {
            var levelNode = node as LevelNode;
            levelNodeDatas.Add(new LevelNodeData
            {
                GUID = levelNode.GUID,
                position = levelNode.GetPosition(),
                anchorList = levelNode.anchorList
            });
            levelND_Dict.Add(levelNode.GUID, new List<LevelNodeLinkData>());
            foreach (var port in levelNode.outputContainer.Children())
            {
                var outputPort = port as Port;
                foreach (Edge edge in outputPort.connections)
                {
                    levelND_Dict[levelNode.GUID].Add(new LevelNodeLinkData
                    {
                        BaseNodeGUID = levelNode.GUID,
                        BasePortName = outputPort.portName,
                        TargetNodeGUID = ((LevelNode)edge.input.node).GUID,
                        TargetPortName = ((Port)edge.input).portName
                    });
                }
            }
        }
        container.levelNodeData = levelNodeDatas;
        container.levelNodeLinkDataDictionary = levelND_Dict;
    }

    public void LoadGraph(LevelNodeContainer container)
    {
        var levelNodeDatas = container.levelNodeData;
        var levelNodeLinkDatas = container.levelNodeLinkDataDictionary;
        Dictionary<string, LevelNode> levelNodeDict = new Dictionary<string, LevelNode>();
        foreach (LevelNodeData nodeData in levelNodeDatas)
        {
            var levelNode = _targetGraphView.CreateAndAddNode(nodeData.DisplayName, nodeData.GUID, nodeData.position, nodeData.anchorList);
            levelNodeDict.Add(nodeData.GUID, levelNode);
        }
        foreach (LevelNode node in levelNodeDict.Values)
        {
            List<LevelNodeLinkData> levelNodeLinkData = levelNodeLinkDatas[node.GUID];
            foreach (var linkData in levelNodeLinkData)
            {
                Port outputPort = node.outputContainer.Children().First(x => x is Port && ((Port)x).portName == linkData.BasePortName) as Port;
                Port inputPort = levelNodeDict[linkData.TargetNodeGUID].inputContainer.Children().First(x => x is Port && ((Port)x).portName == linkData.TargetPortName) as Port;
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
        foreach (LevelNode node in _targetGraphView.nodes.ToList())
        {
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}