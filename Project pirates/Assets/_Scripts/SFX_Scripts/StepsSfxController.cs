using System.Linq;
using UnityEngine;
public class StepsSfxController : MonoBehaviour
{
    private CameraBobbing _cameraBobbing;
    private AudioClips _audioClips;
    private void Awake()
    {
        _cameraBobbing = GetComponent<CameraBobbing>();
        CameraBobbing.OnStepTaken += OnStepTaken;
    }
    private void Start()
    {
        _audioClips = SoundManager.AudioClips;
    }
    private void OnStepTaken()
    {
        AudioClip clip = _audioClips.StepSounds[Random.Range(0, _audioClips.StepSounds.Length)];
        SoundManager.Instance.PlayAudioOneShotAtPosition(clip, transform.position);
    }
}