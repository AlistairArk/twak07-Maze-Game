using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStation : MonoBehaviour{
    
    public float rotationSpeed = 1f;

    private float x;
    private float z;
    private bool rotateX;

    void Start()
    {
        x = 0.0f;
        z = 0.0f;
        // rotateX = true;
        // rotationSpeed = 75.0f;
    }

    void FixedUpdate()    {
            // x += Time.deltaTime * rotationSpeed;



        // transform.localRotation = Quaternion.Euler(x, 0, z);
    }
}
