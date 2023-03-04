using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] public float interactRange = 4f;
    
    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, interactRange))
            print("Something infront of player");
            

        Debug.DrawRay(transform.position, fwd * interactRange, Color.blue);
    }


}
