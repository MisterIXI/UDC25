using UnityEngine;

public class AmbienceSwitcher : MonoBehaviour
{
    [field: SerializeField] private bool IsInside { get; set; } = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsInside)
            {
                if (SoundManager.CurrentMusic != SoundManager.AudioClips.AmbientInside)
                {
                    Debug.Log("Switching to inside ambience");
                    SoundManager.FadeToMusic(SoundManager.AudioClips.AmbientInside);
                }
            }
            else
            {

                if (SoundManager.CurrentMusic != SoundManager.AudioClips.AmbientOutside)
                {
                    Debug.Log("Switching to outside ambience");
                    SoundManager.FadeToMusic(SoundManager.AudioClips.AmbientOutside);
                }
            }
        }
    }
}