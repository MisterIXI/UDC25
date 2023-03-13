using UnityEngine;

public class WebGLDisclaimerHider : MonoBehaviour
{
    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
        }
    }
}