using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "LevelNodeContainer", menuName = "Project pirates/LevelNodeContainer", order = 0)]
public class LevelNodeContainer : ScriptableObject
{
    public List<LevelNodeData> levelNodeData = new List<LevelNodeData>();
    public List<LevelNodeLinkData> levelNodeLinkData = new List<LevelNodeLinkData>();
}
