using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour{
    public Transform target;
    public Vector3 targetOffset;

    void Start(){
        targetOffset = transform.position - target.position;
    }
    void Update(){

        if (target){
            transform.position = Vector3.Lerp(transform.position, target.position+targetOffset, 0.1f);
        }
    }
}