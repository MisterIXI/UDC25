using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotating : MonoBehaviour
{
    [SerializeField]private float speed= 10f;
    private void Rotate()
    {
        transform.RotateAround(transform.parent.position,Vector3.up,speed*Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
}
