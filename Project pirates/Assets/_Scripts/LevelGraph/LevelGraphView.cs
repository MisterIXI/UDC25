using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class LevelGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    public LevelGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("LevelGraphStyle"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());


        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();


        AddElement(GenerateEntryPointNode());
    }

    private LevelNode GenerateEntryPointNode()
    {
        var node = new LevelNode
        {
            title = "Entry Point",
            GUID = System.Guid.NewGuid().ToString(),
            EntryPoint = true
        };
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    private Port GeneratePort(LevelNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    public void CreateNode(string nodeName)
    {
        AddElement(CreateLevelNode(nodeName));
    }
    public LevelNode CreateAndAddNode(string nodeName, string GUID, Rect position, AnchorList anchorList)
    {
        var levelNode = CreateLevelNode(nodeName);
        levelNode.GUID = GUID;
        levelNode.SetPosition(position);
        levelNode.anchorList = anchorList;
        AddElement(levelNode);
        return levelNode;
    }

    private LevelNode CreateLevelNode(string nodeName)
    {
        var levelNode = new LevelNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString()
        };


        var referenceField = new ObjectField
        {
            objectType = typeof(AnchorList),
            value = null,
            allowSceneObjects = false
        };
        referenceField.RegisterValueChangedCallback(evt =>
        {
            levelNode.anchorList = (AnchorList)evt.newValue;
            UpdateOutputCount(levelNode, levelNode.anchorList);
        });
        levelNode.mainContainer.Add(referenceField);

        levelNode.RefreshExpandedState();
        levelNode.RefreshPorts();
        levelNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        return levelNode;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                if (port.direction == startPort.direction)
                    compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }

    public void UpdateOutputCount(LevelNode levelNode, AnchorList anchorlist)
    {
        int anchorCount = anchorlist.anchors.Count;
        levelNode.outputContainer.Clear();
        levelNode.inputContainer.Clear();
        for (int i = 0; i < anchorCount; i++)
        {
            var inputPort = GeneratePort(levelNode, Direction.Input, Port.Capacity.Multi);
            var outputPort = GeneratePort(levelNode, Direction.Output, Port.Capacity.Multi);
            inputPort.portName = anchorlist.anchors[i].name;
            outputPort.portName = anchorlist.anchors[i].name;
            levelNode.inputContainer.Add(inputPort);
            levelNode.outputContainer.Add(outputPort);
        }
        levelNode.RefreshPorts();
        levelNode.RefreshExpandedState();
    }
}