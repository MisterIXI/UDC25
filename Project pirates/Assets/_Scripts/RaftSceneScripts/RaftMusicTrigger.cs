using UnityEngine;

public class RaftMusicTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            SoundManager.FadeToMusic(SoundManager.AudioClips.CreditsMusic);
            Camera.main.GetComponentInChildren<GramophoneBackwardsNoise>()?.gameObject.SetActive(false);
        }
    }
}