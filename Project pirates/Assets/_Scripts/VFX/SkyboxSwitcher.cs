using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{

    [field: SerializeField] private Material SkyboxToSwitchTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (RenderSettings.skybox != SkyboxToSwitchTo)
                RenderSettings.skybox = SkyboxToSwitchTo;
        }
    }
}