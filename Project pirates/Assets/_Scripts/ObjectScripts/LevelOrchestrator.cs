using UnityEngine;

public class LevelOrchestrator : MonoBehaviour
{
    public static LevelOrchestrator Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
    }
}