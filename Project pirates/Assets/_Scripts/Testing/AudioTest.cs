using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public bool testAudio;
    public AudioClip clip;
    // Update is called once per frame
    void Update()
    {
        if (testAudio)
        {
            testAudio = false;
            SoundManager.Instance.PlayAudioOneShotAtPosition(clip, new Vector3(Random.Range(-50,50), Random.Range(-50, 50), Random.Range(-50, 50)));
        }    
    }
}
