using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour{

    public List<GameObject> objectList = new List<GameObject>(); // list of last shown.


    private Camera cam;

    public List<GameObject> prefabList = new List<GameObject>();  // list of objects rays hit
    public List<GameObject> prefabList2 = new List<GameObject>(); // list of objects rays hit last call

    private MazeGenerator MazeGenerator;
    private PlayerManager PlayerManager;

    public bool rayRed = true;
    public bool rayGreen = true;
    public bool rayYellow = true;
    public bool rayBlue = true;
    public bool enableOcclusionCulling = true;

    // Start is called before the first frame update
    void Start(){
        MazeGenerator = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        PlayerManager = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>();
        // cam = GameObject.FindWithTag("StationCam").GetComponent<Camera>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (MazeGenerator.enableCulling && enableOcclusionCulling) OcclusionCulling();
        else if (MazeGenerator.enableCulling) FrustumCulling();
    }


    void OcclusionCulling(){
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
        
        int rayRes = 50;           // Spacing between rays
        if (PlayerManager.enableVR) rayRes*=2;

        int screenX = Screen.width+rayRes; // Pixel width of screen

        bool screenPass = true;
        // Loop across pixels in screen
        while (screenX>0){
            if (PlayerManager.enableVR && screenX>0) screenPass = false;
            else if (PlayerManager.enableVR && screenX*2>0) screenPass = false;

            screenX-=rayRes;
            int screenY;
            if (PlayerManager.enableVR) screenY = Screen.height*2+rayRes;
            else screenY = Screen.height+rayRes;

            while (screenY>-rayRes){
                screenY-=rayRes;

                Vector3 rayPos = new Vector3(screenX, screenY-rayRes, 0);
                Ray ray = cam.ScreenPointToRay(rayPos);
                RaycastHit hit;

                bool rayHit = false;
                int i=0;
                while(i<2){
                    // If object hit / Layermask Default layer
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 0)){ 

                        // If object is tagged as occludable
                        if (hit.transform.gameObject.tag == "Occludable"){ // Ray will stop
                            // If it is not already listed by another ray
                            if (!prefabList.Contains(hit.transform.gameObject)){
                                prefabList.Add(hit.transform.gameObject);

                                if (MazeGenerator.enableDebugRaycast && rayGreen)
                                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                            } else {
                                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.cyan);    
                            }
                            rayHit=true;
                            i=2;
                        }else if (hit.transform.gameObject.tag == "Transparent"){   // Ray can pass through
                            // If it is not already listed by another ray
                            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue);

                        }else{  // Ray will stop
                            // Draw ray - Object is not occludable
                            if (MazeGenerator.enableDebugRaycast && rayYellow)
                                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                            rayHit=true;
                            i=2;
                        }
                        // i++;
                    }else{
                        // Draw ray - ray has no contacts
                        if (MazeGenerator.enableDebugRaycast && rayRed)
                            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
                    }
                    i++;
                }
            }
        }

        RenderObject();  
    }


    void FrustumCulling(){

        prefabList.Clear();         // List objects rays hit
        int rayRes = 10;            // Spacing between rays
        int screenX = Screen.width-(int)((double)(Screen.width+rayRes)*.5); // Pixel width of screen

        // Loop across pixels in screen
        while (screenX>0){

            screenX-=rayRes;
            int screenY = Screen.height-(int)((double)(Screen.height+rayRes)*.5);

            while (screenY>-rayRes){
                screenY-=rayRes;


                Vector3 rayPos = new Vector3(screenX, screenY-rayRes, 0);
                // Ray ray = cam.ScreenPointToRay(rayPos);
                // RaycastHit[] hits = Physics.RaycastAll(ray, out hits, Mathf.Infinity);
                
                Ray ray = cam.ScreenPointToRay(rayPos);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray); // .OrderBy(h=>h.distance).ToArray();

                // // Order hits in terms of distance
                // System.Array.Sort(hits, (x,y) => x.distance.CompareTo(y.distance));

                if (hits.Length>0){ // If object hit

                    int i = 0;
                    while (i < hits.Length) {
                        RaycastHit hit = hits[i];
                        // Debug.Log (hit.collider.gameObject.name);
                        // If object is tagged as occludable
                        if (hit.transform.gameObject.tag == "Occludable"){ // Ray will stop
                            // If it is not already listed by another ray
                            if (!prefabList.Contains(hit.transform.gameObject)){
                                prefabList.Add(hit.transform.gameObject);
                                // if (MazeGenerator.enableDebugRaycast && rayGreen)
                                //     Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                            }
                            // i = hits.Length;
                        }else if (hit.transform.gameObject.tag == "Transparent"){   // Ray can pass through
                            // If it is not already listed by another ray
                            // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue);
                        }else{  // Ray will stop
                            // Draw ray - Object is not occludable
                            // if (MazeGenerator.enableDebugRaycast && rayYellow)
                            //     Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                            // i = hits.Length;
                        }
                        i++;
                    }
                }else{
                    // Draw ray - ray has no contacts
                    if (MazeGenerator.enableDebugRaycast && rayRed)
                        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

                }
            }
        }
        RenderObject();
    }


    public void RenderObject(){
        // Iterate over list of objects rays hit in the CURRENT call
        foreach (GameObject cell in prefabList){     // If the object was not added to the list last call,
            if (!prefabList2.Contains(cell)){        // it must be inactive - hence, activate it.
                cell.GetComponent<Renderer>().enabled = true; 

                foreach(Transform tr in cell.transform.parent){
                    if(tr.tag == "OccludableOther"){
                        tr.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        // Iterate over list of objects rays hit in the LAST call
        foreach (GameObject cell in prefabList2){
            if (!prefabList.Contains(cell) && cell!=null){         //  If the object can no longer be found - cull it
                // Debug.Log(cell.name);
                cell.GetComponent<Renderer>().enabled = false;

                foreach(Transform tr in cell.transform.parent){
                    if(tr.tag == "OccludableOther"){
                        tr.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
        // Set new list of currently rendered objects       
        prefabList2 = new List<GameObject>(prefabList); 
        
    }
}



        // GameObject[] prefabs;
        // prefabs = GameObject.FindGameObjectsWithTag("Occludable");
     

        // foreach (GameObject prefab in prefabs){
        //     if (prefab.name=="Occludable"){
        //         // print("checking");
        //         if (prefab.transform.parent.transform.GetComponent<Renderer>().isVisible){
        //             prefab.GetComponent<Renderer>().enabled = true;
        //             // Debug.Log("visible");
        //         }else{
        //             // Debug.Log("NOT Visible");
        //             prefab.transform.GetComponent<Renderer>().enabled = false;
        //         }
        //     }
        // }