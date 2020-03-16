using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour{

    public bool goalTrigger = false; 

	void OnTriggerEnter(Collider other){
        goalTrigger = true;
    }
}
