using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "Project pirates/AudioClips", order = 0)]
public class AudioClips : ScriptableObject
{
    [field: Header("AudioClips")]
    [field: Header("General")]
    [field: SerializeField] public AudioClip[] StepSounds { get; private set; }
    [field: SerializeField] public AudioClip CollisionObject { get; private set; }
    [field: SerializeField] public AudioClip DoorOpenLong { get; private set; }
    [field: SerializeField] public AudioClip DoorOpenShort { get; private set; }

    [field: Header("Menu")]
    [field: SerializeField] public AudioClip ButtonHover { get; private set; }
    [field: SerializeField] public AudioClip ButtonPress { get; private set; }

    [field: Header("Other")]
    [field: SerializeField] public AudioClip GrammophonMusic { get; private set; }
    [field: SerializeField] public AudioClip SomethingHappend { get; private set; }

    [field: Header("Music and Ambient")]
    [field: SerializeField] public AudioClip MenuMusic { get; private set; }
    [field: SerializeField] public AudioClip AmbientInside { get; private set; }
    [field: SerializeField] public AudioClip AmbientOutside { get; private set; }
}