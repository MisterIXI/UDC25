using UnityEngine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [field: SerializeField] public GameObject VirtualCamera { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameManager.OnSceneSwitched += OnSceneSwitched;
    }

    private void OnSceneSwitched(int buildIndex)
    {
        if (buildIndex == 1)
        {
            VirtualCamera.GetComponent<PlayerCameraController>().enabled = true;
            VirtualCamera.GetComponent<CineMachineSettings>().enabled = true;
        }
    }
}