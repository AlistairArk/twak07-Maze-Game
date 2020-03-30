using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour{

    private Camera cam;
    public GameObject crosshairA;
    public GameObject crosshairB;


    void Start(){
        cam = gameObject.GetComponent<Camera>();
        crosshairB.SetActive(false);
    }


    void Update(){

        // Cast ray from center of display
        float screenX = Screen.width;
        float screenY = Screen.height;

        Vector3 rayPos = new Vector3(screenX*.5f, screenY*.5f, 0);
        Ray ray = cam.ScreenPointToRay(rayPos);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){ // If object hit

            // If object is tagged as terminal
            if (hit.transform.gameObject.tag == "Terminal"){
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                crosshairA.SetActive(false);
                crosshairB.SetActive(true);

                // On left mouse click
                if(Input.GetMouseButtonDown(0)) DoorTrigger(hit.transform.gameObject);
            }else{
                crosshairA.SetActive(true);
                crosshairB.SetActive(false);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
            }
        }
    }


    void DoorTrigger(GameObject terminal){
        terminal.GetComponent<TerminalTrigger>().trigger = true; // Trigger door script
    }
}
