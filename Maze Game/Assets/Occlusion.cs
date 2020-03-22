using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour{

    public List<GameObject> objectList = new List<GameObject>(); // list of last shown.

    public GameObject pcCam;
    private Camera cam;

    public List<GameObject> prefabList = new List<GameObject>(); // list of objects to show.
    public List<GameObject> prefabList2 = new List<GameObject>(); // list of last shown.

    // Start is called before the first frame update
    void Start(){
        cam = pcCam.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        float fovAngle = 90f;
        int rayRes = 100;
        int screenX = Screen.width;

        prefabList.Clear();   // List objects to show

        print("list coppied prior " + prefabList2.Count);

        while (screenX>0){
            screenX-=rayRes;
            int screenY = Screen.height;
            while (screenY>0){
                screenY-=rayRes;
                Vector3 rayPos = new Vector3(screenX, screenY, 0);
                Ray ray = cam.ScreenPointToRay(rayPos);
                
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)){ // If Hit
                    // Debug.Log("Hitting: "+ hit.transform.gameObject.name);
                    // Debug.DrawRay(cam., rayPos, Color.yellow);
                    // Debug.Log("Hit: "+hit.transform.gameObject.name);

                    if (hit.transform.gameObject.tag == "Prefab" && !prefabList.Contains(hit.transform.gameObject)){
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                        prefabList.Add(hit.transform.gameObject);
                    }else{
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                    }

                }else{
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

                }
            }
        }


        foreach (GameObject cell in prefabList){
            Debug.Log("showing: "+cell.name);
            cell.transform.GetChild(0).gameObject.SetActive(true);
        }

        foreach (GameObject cell2 in prefabList2){
            foreach (GameObject cell in prefabList){
                print("Does: "+cell.name+"=="+cell2.name);
            }
        }
            // Debug.Log("not present in list: "+cell.name);
            // if (!prefabList.Contains(cell)) cell.transform.GetChild(0).gameObject.SetActive(false);



        // print("list coppied from "+ prefabList2.Count + " to " + prefabList2.Count);

        foreach (GameObject cell in prefabList)
            if (!prefabList2.Contains(cell))
                cell.transform.GetChild(0).gameObject.SetActive(true); 
        

        foreach (GameObject cell in prefabList2)
            if (!prefabList.Contains(cell))
                cell.transform.GetChild(0).gameObject.SetActive(false);
        
        prefabList2 = new List<GameObject>(prefabList); 
    }
}


        // Vector3 rayPosition = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        // Vector3 rayRotation = Quaternion.AngleAxis(-fovAngle, cam.transform.up) * cam.transform.forward;
        // Ray rayCenter = new Ray(rayPosition, rayRotation);

        // RaycastHit hit;
        // // Does the ray intersect any objects excluding the player layer
        // if (Physics.Raycast(rayCenter, out hit, Mathf.Infinity)){
        //     Debug.DrawRay(rayPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //     Debug.Log("Did Hit");
        
        // }else{
        //     Debug.DrawRay(rayPosition, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //     Debug.Log("Did not Hit");
        // }
    // RaycastHit[] hits;
    // hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);
    // RaycastHit hit;

    // Ray ray = pcCam.ScreenPointToRay(Input.mousePosition);
    // if (Physics.Raycast(ray, out hit)) {
    //     Transform objectHit = hit.transform;

    //     foreach (RaycastHit hit2 in hits){
    //         print(hit2.transform.gameObject.SetActive(false));
    //     }
    // }

    // //Setting up Vector3's for rays
    // Vector3 rayPosition = new Vector3(transform.position.x, headHeight, transform.position.z);
    // Vector3 leftRayRotation = Quaternion.AngleAxis(-fovAngle, transform.up) * transform.forward;
    // Vector3 rightRayRotation = Quaternion.AngleAxis(fovAngle, transform.up) * transform.forward;

    // //Constructing rays
    // Ray rayCenter = new Ray(rayPosition, transform.forward);
    // Ray rayLeft = new Ray(rayPosition, leftRayRotation);
    // Ray rayRight = new Ray(rayPosition, rightRayRotation);

    // Debug.DrawRay(contact.point, contact.normal, Color.red, 5.0f);