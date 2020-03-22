using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour{
    /* This function alters the position of the earth to create 
    the illusion that it's large and far away */

    public GameObject pc;
    public float distance;

    [Tooltip("Planet axis in world vector, defaults to start up vector")]
    public Vector3 Axis;

    [Range(-1000.0f, 1000.0f)]
    [Tooltip("Rotation speed around axis")]
    public float orbitalSpeed = 0f;
    [Range(-1000.0f, 1000.0f)]
    [Tooltip("Rotation speed around axis")]
    public float stationRotation = 10f;



    private void OnEnable(){
        if (Axis == Vector3.zero){
            Axis = transform.up;
        }else{
            Axis = Axis.normalized;
        }
    }


    void Start(){
        distance = pc.transform.position.x - transform.position.x;
    }


    void Update(){
        // Move planet relative to player to create size illusion
        transform.position = new Vector3(pc.transform.position.x-distance,pc.transform.position.y,pc.transform.position.z);

        gameObject.transform.parent.Rotate(Vector3.right * stationRotation * Time.deltaTime);
        transform.Rotate(Axis, orbitalSpeed * Time.deltaTime);

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * orbitalSpeed);
        RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(0f,orbitalSpeed,0f));

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * stationRotation);
        RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,0f,0f));
        // RenderSettings.skybox.SetVector("_RotationAxis", Axis);
    }
}

        // transform.localRotation = Quaternion.Euler(0f, skyboxRotationSpeed, 0f);
        // gameObject.transform.Rotate(Vector3.right * stationRotation * Time.deltaTime);

        // RenderSettings.skybox.SetFloat("_Rotation", Time.time * orbitalSpeed);
        // RenderSettings.skybox.SetVector("_RotationAxis", new Vector3(stationRotation,orbitalSpeed,0));


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