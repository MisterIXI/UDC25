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
        // raycast below
        RaycastHit hit;
        AudioClip clip = null;
        // raycast on every layer except player


        // if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        // {
        //     // Debug.Log("Steps raycast hit: " + hit.collider.name);
        //     // if hit ground
        //     if (hit.collider.CompareTag("GrasGround"))
        //     {
        //         clip = _audioClips.StepSoundsGrass[Random.Range(0, _audioClips.StepSoundsGrass.Length)];
        //     }
        //     else
        //     {
        //         clip = _audioClips.StepSounds[Random.Range(0, _audioClips.StepSounds.Length)];
        //     }
        // }
                clip = _audioClips.StepSounds[Random.Range(0, _audioClips.StepSounds.Length)];

        SoundManager.Instance.PlayAudioOneShotAtPosition(clip, transform.position);
    }
}