using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "Project pirates/AudioClips", order = 0)]
public class AudioClips : ScriptableObject
{
    [field: Header("AudioClips")]
    [field: SerializeField] public AudioClip[] StepSounds { get; private set; }
}