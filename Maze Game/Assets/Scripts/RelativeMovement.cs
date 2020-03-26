using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour{
    /* This function alters the position of the earth to create 
    the illusion that it's large and far away */

    public GameObject skyboxCamera;
    public GameObject playerCamera;
    public GameObject skyboxPlayerRig;
    public GameObject earth;

    [Tooltip("Distance between the planet and the camera")]
    public float distance = 50f;

    [Tooltip("Planet axis in world vector, defaults to start up vector")]
    public Vector3 Axis;

    [Range(-1000.0f, 1000.0f)]
    [Tooltip("Rotation speed around axis")]
    public float orbitalRotation = 10f;
    [Range(-1000.0f, 1000.0f)]
    [Tooltip("Rotation speed around axis")]
    public float stationRotation = 10f;




    private void OnValidate(){
        if (Axis == Vector3.zero){
            Axis = transform.up;
        }else{
            Axis = Axis.normalized;
        }

        skyboxCamera.transform.localPosition = new Vector3(distance,
            skyboxCamera.transform.localPosition.y,
            skyboxCamera.transform.localPosition.z);
    }




    void Update(){
        skyboxCamera.transform.RotateAround(earth.transform.position, Vector3.down, orbitalRotation * Time.deltaTime);
        skyboxCamera.transform.Rotate(Vector3.left * stationRotation * Time.deltaTime);
        skyboxPlayerRig.transform.localRotation = playerCamera.transform.rotation;

    }
}


