using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour{

    public List<GameObject> objectList = new List<GameObject>(); // list of last shown.

    public GameObject pcCam;
    private Camera cam;

    public List<GameObject> prefabList = new List<GameObject>();  // list of objects rays hit
    public List<GameObject> prefabList2 = new List<GameObject>(); // list of objects rays hit last call

    // Start is called before the first frame update
    void Start(){
        cam = pcCam.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate(){

        /*
        rayRes - The amount of spacing between each ray
                Higher means more precision - but at the cost of performance.
                We may want to add bias to ray placement so they are focused 
                on the edges of the screen. This will decrease chance of "object pop in"
                for minimal performance cost.

                Boosting screenX/screenY above the natural res may also do the trick as
                this would allow the rays to work with a higher degree of fov than the
                player camera.

        */
        prefabList.Clear();         // List objects rays hit
        
        int rayRes = 100;           // Spacing between rays
        int screenX = Screen.width; // Pixel width of screen

        // Loop across pixels in screen
        while (screenX>0){

            screenX-=rayRes;
            int screenY = Screen.height;

            while (screenY>0){
                screenY-=rayRes;

                Vector3 rayPos = new Vector3(screenX, screenY, 0);
                Ray ray = cam.ScreenPointToRay(rayPos);
                
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)){ // If object hit

                    // If object is tagged as occludable
                    if (hit.transform.gameObject.tag == "Prefab"){
                        // If it is not already listed by another ray
                        if (!prefabList.Contains(hit.transform.gameObject)){
                            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                            prefabList.Add(hit.transform.gameObject);
                        }
                    }else{
                        // Draw ray - Object is not occludable
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                    }

                }else{
                    // Draw ray - ray has no contacts
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

                }
            }
        }

        // Iterate over list of objects rays hit in the CURRENT call
        foreach (GameObject cell in prefabList)     // If the object was not added to the list last call,
            if (!prefabList2.Contains(cell))        // it must be inactive - hence, activate it.
                cell.GetComponent<Renderer>().enabled = true; 
        
        // Iterate over list of objects rays hit in the LAST call
        foreach (GameObject cell in prefabList2)
            if (!prefabList.Contains(cell))         //  If the object can no longer be found - cull it
                cell.GetComponent<Renderer>().enabled = false;
        
        // Set new list of currently rendered objects       
        prefabList2 = new List<GameObject>(prefabList); 
    }
}