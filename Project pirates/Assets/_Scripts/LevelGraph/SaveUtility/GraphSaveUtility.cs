using System;
using System.Collections.Generic;
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
        var levelNodeLinkDatas = new List<LevelNodeLinkData>();
        foreach (var node in _targetGraphView.nodes)
        {
            var levelNode = node as LevelNode;
            levelNodeDatas.Add(new LevelNodeData
            {
                GUID = levelNode.GUID,
                position = levelNode.GetPosition(),
                anchorList = levelNode.anchorList
            });
            foreach (var port in levelNode.outputContainer.Children())
            {
                var outputPort = port as Port;
                foreach(Edge edge in outputPort.connections)
                {
                    levelNodeLinkDatas.Add(new LevelNodeLinkData
                    {
                        BaseNodeGUID = levelNode.GUID,
                        BasePortName = outputPort.portName,
                        TargetNodeGUID = ((LevelNode)edge.input.node).GUID,
                        TargetPortName = ((Port)edge.input).portName
                    });
                }
            }
        }
    }

    public void LoadGraph(LevelNodeContainer container)
    {
        var levelNodeDatas = container.levelNodeData;
        var levelNodeLinkDatas = container.levelNodeLinkData;

        foreach(LevelNodeData nodeData in levelNodeDatas)
        {
            //TODO: CONTINUE HERE
            // var levelNode = _targetGraphView.CreateAndAddNode(nodeData.)
        }
    }
}