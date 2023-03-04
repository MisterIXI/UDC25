using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] public float interactRange = 4f;

    private Camera mainCamera;

    private void Start() 
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 fwd = mainCamera.transform.forward;

        if (Physics.Raycast(mainCamera.transform.position, fwd, interactRange))
            print("Something infront of player");
            

        Debug.DrawRay(mainCamera.transform.position, fwd * interactRange, Color.blue);
    }
}