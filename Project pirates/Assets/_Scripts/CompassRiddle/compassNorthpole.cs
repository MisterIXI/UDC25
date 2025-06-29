using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compassNorthpole : MonoBehaviour
{
    [SerializeField] Transform northpole;
    private Quaternion startRotation;
    [SerializeField]private float speed;


    void Start()
    {
        startRotation = gameObject.transform.rotation;
    }


    void FixedUpdate()
    {
        Vector3 direction = northpole.position - transform.position; // rotation difference
        Vector3 projection = Vector3.ProjectOnPlane(direction, transform.parent.up);
        Quaternion rotation = Quaternion.LookRotation(projection, transform.parent.up); // rot dif

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * speed); 
    }
}
