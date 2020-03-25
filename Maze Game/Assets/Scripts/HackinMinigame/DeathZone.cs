using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour{

    public GameObject ball;     
    public bool fail = false;   // Identifies if the ball fell of the map


    /*
    Note: OnTriggerEnter is used but tracking ball y pos may also work.

    - I have plans for layered maps for amping up the difficulty of mazes
    - Not sure which approach would work best just yet. 
    */
    void OnTriggerEnter(Collider other){
        if (other.transform.gameObject==ball) fail = true;
    }
}
