using System;
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

        CreateGridBackground();
    }

    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    public void CreateNode()
    {
        AddElement(CreateLevelNode("LevelNode"));
    }
    public void CreateDecisionNode()
    {
        AddElement(CreateDecisionNode("Decision"));
    }
    public void CreateLinkNode()
    {
        AddElement(CreateLinkNode("Link"));
    }
    public LevelNode CreateAndAddLevelNode(string nodeName, string GUID, Rect position, AnchorList anchorList)
    {
        var levelNode = CreateLevelNode(nodeName, GUID);
        levelNode.SetPosition(position);
        UpdateOutputCount(levelNode, anchorList);
        levelNode.anchorList = anchorList;
        levelNode.RefreshExpandedState();
        levelNode.RefreshPorts();
        levelNode.mainContainer.Q<ObjectField>().value = anchorList;
        AddElement(levelNode);
        return levelNode;
    }

    public DecisionNode CreateAndAddDecisionNode(string nodeName, string GUID, Rect position, string flagName)
    {
        var decisionNode = CreateDecisionNode(nodeName, GUID);
        decisionNode.SetPosition(position);
        decisionNode.flagName = flagName;
        decisionNode.RefreshExpandedState();
        decisionNode.RefreshPorts();
        decisionNode.mainContainer.Q<TextField>().value = flagName;
        AddElement(decisionNode);
        return decisionNode;
    }

    public LinkNode CreateAndAddLinkNode(string nodeName, string GUID, Rect position, NodeContainer container)
    {
        var linkNode = CreateLinkNode(nodeName, GUID);
        linkNode.SetPosition(position);
        linkNode.container = container;
        linkNode.RefreshExpandedState();
        linkNode.RefreshPorts();
        linkNode.mainContainer.Q<ObjectField>().value = container;
        AddElement(linkNode);
        return linkNode;
    }


    private LevelNode CreateLevelNode(string nodeName, string GUID = null)
    {
        var levelNode = new LevelNode
        {
            title = nodeName,
            GUID = GUID == null ? System.Guid.NewGuid().ToString() : GUID
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

    private DecisionNode CreateDecisionNode(string nodeName, string GUID = null)
    {
        var decisionNode = new DecisionNode
        {
            title = nodeName,
            GUID = GUID == null ? System.Guid.NewGuid().ToString() : GUID
        };
        var textField = new TextField
        {
            name = "FlagName",
            value = "FlagName"
        };
        textField.RegisterValueChangedCallback(evt =>
        {
            decisionNode.flagName = evt.newValue;
        });
        decisionNode.mainContainer.Add(textField);
        var inputPort = GeneratePort(decisionNode, Direction.Input, Port.Capacity.Single);
        var outputPortTrue = GeneratePort(decisionNode, Direction.Output, Port.Capacity.Single);
        var outputPortFalse = GeneratePort(decisionNode, Direction.Output, Port.Capacity.Single);
        inputPort.portName = "Input";
        outputPortTrue.portName = "True";
        outputPortFalse.portName = "False";
        decisionNode.inputContainer.Add(inputPort);
        decisionNode.outputContainer.Add(outputPortTrue);
        decisionNode.outputContainer.Add(outputPortFalse);
        decisionNode.RefreshExpandedState();
        decisionNode.RefreshPorts();
        decisionNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        return decisionNode;
    }

    private LinkNode CreateLinkNode(string nodeName, string GUID = null)
    {
        var linkNode = new LinkNode
        {
            title = nodeName,
            GUID = GUID == null ? System.Guid.NewGuid().ToString() : GUID
        };
        var containerField = new ObjectField
        {
            objectType = typeof(NodeContainer),
            value = null,
            allowSceneObjects = false
        };
        containerField.RegisterValueChangedCallback(evt =>
        {
            linkNode.container = (NodeContainer)evt.newValue;
        });
        linkNode.mainContainer.Add(containerField);
        var inputPort = GeneratePort(linkNode, Direction.Input, Port.Capacity.Single);
        var outputPort = GeneratePort(linkNode, Direction.Output, Port.Capacity.Single);
        inputPort.portName = "Link";
        outputPort.portName = "Link";
        linkNode.inputContainer.Add(inputPort);
        linkNode.outputContainer.Add(outputPort);
        linkNode.RefreshExpandedState();
        linkNode.RefreshPorts();
        linkNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
        return linkNode;
    }
    public void CreateGridBackground()
    {
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                if (port.direction != startPort.direction)
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