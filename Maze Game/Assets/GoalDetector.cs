using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour{

	public GameObject mapGen;
	private wallPlacer WallPlacer;


    // Start is called before the first frame update
    void Start(){
        WallPlacer   = mapGen.GetComponent<wallPlacer>();
        WallPlacer.startMap();
    }




	void OnTriggerEnter(Collider other){
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
