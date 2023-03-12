using UnityEngine;

[CreateAssetMenu(fileName = "GramophoneSettings", menuName = "Project pirates/GramophoneSettings", order = 0)]
public class GramophoneSettings : ScriptableObject
{
    [field: Header("GramophoneFollowNoise")]
    [field: SerializeField] public bool DrawDebugGizmos { get; private set; } = false;
    [field: SerializeField] public float GramoFollowSpeed { get; private set; } = 7f;
    [field: Header("GramophoneBackwardsNoise")]
    [field: SerializeField] public float GramoBackwardsNoiseTriggerDuration { get; private set; } = 5f;
    [field: SerializeField][field: Range(0, 10)] public int GramoBackwardsMinimumNodeChanges { get; private set; } = 3;
}