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

        // skyboxCamera.transform.Rotate(Vector3.up * stationRotation * Time.deltaTime);
        // skyboxCamera.transform.Rotate(Vector3.down * stationRotation * Time.deltaTime);
        // skyboxCamera.transform.Rotate(Vector3.right * stationRotation * Time.deltaTime);

        // distance = pc.transform.position.x - transform.position.x;
        // Transform parent = gameObject.transform.parent;
        // Transform root = gameObject.transform.root;

        // orbitalRotation = 1f;
        // stationRotation = 1f;

        // // Move planet relative to player to create size illusion
        // parent.localPosition = new Vector3(pc.transform.position.x-distance,pc.transform.position.y,pc.transform.position.z);

        // parent.Rotate(Vector3.right * stationRotation * Time.deltaTime);
        // // root.Rotate(Vector3.down * orbitalRotation * Time.deltaTime);
        // transform.Rotate(Axis, orbitalRotation * Time.deltaTime);

        // // RenderSettings.skybox.SetFloat("_Rotation", Time.time * orbitalRotation+stationRotation);
        // // Vector3 stationVector = new Vector3(stationRotation,0f,0f);
        // // Vector3 orbitalVector = new Vector3(0f,stationRotation,0f);

        // // skyboxVector = stationVector * orbitalVector;
        // // RenderSettings.skybox.SetVector("_RotationAxis", skyboxVector);
        // // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,0f,0f));
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(0f,orbitalRotation,0f));

    // public float counter = 0f;

    // private bool axisSwitch = false;


        // transform.Rotate(Vector3.right * stationRotation * Time.deltaTime);
        // RenderSettings.skybox.SetFloat("_Rotation", Time.time * orbitalRotation);
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(0f,orbitalRotation,0f));

        // RenderSettings.skybox.SetFloat("_Rotation", Time.time * stationRotation);
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,0f,0f));
        
        // if (axisSwitch) RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(0f,orbitalRotation,0f));
        // else RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,0f,0f));
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,0f,-stationRotation)+new Vector3(0f,orbitalRotation,0f));
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,-orbitalRotation*.5f,-orbitalRotation*2));
        // RenderSettings.skybox.SetVector("_RotationAxis", -Axis);

        // axisSwitch = !axisSwitch; //flip switch
        
        // RenderSettings.skybox.SetVector("_RotationAxis", Axis);

        // transform.localRotation = Quaternion.Euler(0f, skyboxRotationSpeed, 0f);
        // gameObject.transform.Rotate(Vector3.right * stationRotation * Time.deltaTime);

        // RenderSettings.skybox.SetFloat("_Rotation", Time.time * orbitalRotation);
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,orbitalRotation,0));


//         [Range(-1000.0f, 1000.0f)]
//         [Tooltip("Rotation speed around axis")]
//         public float RotationSpeed = 1.0f;

//         [Tooltip("Planet axis in world vector, defaults to start up vector")]
//         public Vector3 Axis;

//         [Tooltip("The sun, defaults to first dir light")]
//         public Light Sun;

//         private MeshRenderer meshRenderer;
//         private MaterialPropertyBlock materialBlock;


//         protected override void Update()
//         {
//             base.Update();

//             if (materialBlock != null && Sun != null && meshRenderer != null)
//             {
//                 meshRenderer.GetPropertyBlock(materialBlock);
//                 materialBlock.SetVector("_SunDir", -Sun.transform.forward);
//                 materialBlock.SetVector("_SunColor", new Vector4(Sun.color.r, Sun.color.g, Sun.color.b, Sun.intensity));
//                 meshRenderer.SetPropertyBlock(materialBlock);
//             }

// #if UNITY_EDITOR

//             if (Application.isPlaying)
//             {

// #endif

//                 transform.Rotate(Axis, RotationSpeed * Time.deltaTime);

// #if UNITY_EDITOR

//             }

// #endif

//         }
//     }
// }