using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour {

	float rotationSpeed = 50f;

    void FixedUpdate(){
    	gameObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

}
