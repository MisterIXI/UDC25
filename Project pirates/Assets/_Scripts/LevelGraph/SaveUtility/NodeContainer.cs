using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "LevelNodeContainer", menuName = "Project pirates/LevelNodeContainer", order = 0)]
public class NodeContainer : ScriptableObject
{
    public List<LevelNodeData> levelNodeData = new List<LevelNodeData>();
    public List<DecisionNodeData> decisionNodeData = new List<DecisionNodeData>();
    public List<LinkNodeData> linkNodeData = new List<LinkNodeData>();
    public List<NodeLinkData> NodeLinkData = new List<NodeLinkData>();
}
