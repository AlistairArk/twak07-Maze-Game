using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour{

	float rotationSpeed = 50f;
	public GameObject mapGen;
	private wallPlacer WallPlacer;


    // Start is called before the first frame update
    void Start(){
        WallPlacer   = mapGen.GetComponent<wallPlacer>();
        WallPlacer.startMap();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void FixedUpdate(){
    	gameObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

	void OnTriggerEnter(Collider other){
        Debug.Log("TRIGGER");
        WallPlacer.resetMap();
        // WallPlacer
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }
        // if (collision.relativeVelocity.magnitude > 2)
        //     audioSource.Play();
    }
}
