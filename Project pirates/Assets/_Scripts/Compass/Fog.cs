using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [Range (0f, 0.1f)] public float fogDensity = 0.06f;


    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0f;
    }


    void FixedUpdate()
    {
        if (RenderSettings.fogDensity != fogDensity)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDensity, Time.deltaTime);
        }
    }
}
 