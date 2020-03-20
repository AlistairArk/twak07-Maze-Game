using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour{
    /* This function alters the position of the earth to create 
    the illusion that it's large and far away */

    public GameObject pc;
    public float distance;

    // Start is called before the first frame update
    void Start(){
        distance = pc.transform.position.x - transform.position.x;
    }

    // Update is called once per frame
    void Update(){
        transform.position = new Vector3(pc.transform.position.x-distance,pc.transform.position.y,pc.transform.position.z);
    }
}
