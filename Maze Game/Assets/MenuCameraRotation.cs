using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRotation : MonoBehaviour{

    public bool rotate = true;
    public float rotationSpeed = .01f;

    // Update is called once per frame
    void FixedUpdate(){
        if (rotate) transform.Rotate(0f,rotationSpeed,0f);
    }
}
