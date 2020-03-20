using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDistanceHandler : MonoBehaviour{


    public GameObject pcObject;
    public GameObject pcCamera;
    private Camera pcCam;
    public Vector3 pcPos; 
    public Vector3 pcRot; 
    public int pcX;         // co-ords of the player's current maze cell
    public int pcZ;         // co-ords of the player's current maze cell
    public int pcX2=0;      // co-ords of the player's last maze cell
    public int pcZ2=0;      // co-ords of the player's last maze cell

    public int facing=0;    // Direction the player is currently facing
    public int facing2=0;   // Direction the player was last facing
    
    public List<GameObject> cellList = new List<GameObject>(); // list of objects to show.
    public List<GameObject> cellList2 = new List<GameObject>(); // list of last shown.

    private WallPlacer WallPlacer;



    void Start(){
        WallPlacer = gameObject.GetComponent<WallPlacer>();
        
        pcCam = pcCamera.GetComponent<Camera>();
    }


    // void FixedUpdate(){
    //     RaycastHit[] hits;
    //     hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

    //     RaycastHit hit;
    //     Ray ray = pcCam.ScreenPointToRay(Input.mousePosition);
        
    //     if (Physics.Raycast(ray, out hit)) {
    //         Transform objectHit = hit.transform;
    //         // objectHit.gameObject.SetActive(false);
    //         foreach (RaycastHit hit2 in hits){
    //             print(hit2.transform.gameObject.SetActive(false));
    //         }
    //     }
    // }
        // Do something with the object that was hit by the raycast.
        // Debug.DrawRay(transform.position, transform.forward, Color.red);



        // foreach (RaycastHit hit in hits){
        //     if (Physics.Raycast(ray, out hit)) {
        //         Transform objectHit = hit.transform;
        //         objectHit.gameObject.SetActive(false);
        //     }
        // }
    










    // void FixedUpdate(){
    //     bool visibleComponent = false;

    //     foreach (Transform child in WallPlacer.prefabMazeParent.transform){
    //         visibleComponent=false;
    //         foreach (Renderer renderer in child.gameObject.GetComponentsInChildren<Renderer>()){
    //             if (renderer.IsVisibleFrom(pcCam)){
    //                 // // && RaycastHit.collider.gameObject){
    //                 // RaycastHit hit;
    //                 // Ray ray = pcCam.ScreenPointToRay(Input.mousePosition);
    //                 // if (Physics.Raycast(ray, out hit))
    //                 //     if (hit.collider != null)
    //                 //         hit.collider.enabled = false;

    //                 visibleComponent=true;
    //                 break;
    //             }else{
    //                 break;
    //             }
    //         }
    //         child.gameObject.SetActive(visibleComponent);
    //     }
    // }



        // for (int i = 0; i < hits.Length; i++){
        //     RaycastHit hit = hits[i];

        //     hit.transform.gameObject.SetActive(false);
        //     print("hit: "+hit);
        //     // Renderer rend = hit.transform.GetComponent<Renderer>();

        //     // if (rend){

        //         // // Change the material of all hit colliders
        //         // // to use a transparent shader.
        //         // rend.material.shader = Shader.Find("Transparent/Diffuse");
        //         // Color tempColor = rend.material.color;
        //         // tempColor.a = 0.3F;
        //         // rend.material.color = tempColor;
        //     // }
        // }
        // Debug.DrawRay(transform.position, transform.forward, Color.red);



    // using UnityEngine;
     
    // public class TestRendered : MonoBehaviour
    // {   
    //     void Update()
    //     {
    //         if (renderer.IsVisibleFrom(Camera.main)) Debug.Log("Visible");
    //         else Debug.Log("Not visible");
    //     }
    // }



    // void FixedUpdate(){
    //     pcPos = pcObject.transform.position;
    //     pcRot = pcObject.transform.rotation.eulerAngles;
    //     pcX = (int)(Mathf.Floor(pcPos.x*0.1f)); // Round player position to map cell
    //     pcZ = (int)(Mathf.Floor(pcPos.z*0.1f)); // Round player position to map cell

    //     GetDirection();
        
    //     // If, cell changed or player rotates
    //     if (pcX!=pcX2 || pcZ!=pcZ2 || facing!=facing2){
    //         facing2 = facing;
    //         DrawCells();
    //     }
    // }

    // void GetDirection(){ // Get the direction the player is currently facing
    //     if (pcRot.y<45f || pcRot.y>315f){
    //         facing=0;
    //     }else if (pcRot.y>45f && pcRot.y<135f){
    //         facing=1;
    //     }else if (pcRot.y>135f && pcRot.y<225f){
    //         facing=2;
    //     }else{
    //         facing=3;
    //     }
    // }

    // void DrawCells(){
    //     // bool visibleComponent = false;

    //     foreach (Transform child in WallPlacer.prefabMazeParent.transform){
    //         // visibleComponent=false;
    //         Renderer renderer = child.gameObject.GetComponent<Renderer>();
    //         // foreach (Renderer renderer in child.gameObject.GetComponentsInChildren<Renderer>()){
    //             if (renderer.IsVisibleFrom(pcCam)) child.gameObject.SetActive(true);
    //             else child.gameObject.SetActive(false);
    //         // }
            
    //     }
    // }





    // void Update(){
    //     pcPos = pcObject.transform.position;
    //     pcRot = pcObject.transform.rotation.eulerAngles;
    //     pcX = (int)(Mathf.Floor(pcPos.x*0.1f)); // Round player position to map cell
    //     pcZ = (int)(Mathf.Floor(pcPos.z*0.1f)); // Round player position to map cell

    //     GetDirection();

    //     if (pcX!=pcX2 || pcZ!=pcZ2){ // If, cell changed
    //         print("Cell changed");
    //         // if (pcX >= 0 && pcX <= WallPlacer.gridX && pcZ >= 0 && pcZ <= WallPlacer.gridZ){
    //         try{
    //             switch(WallPlacer.cellWalls[pcX][pcZ][6]){
    //                 case(0): // Is corridor
    //                 print("In corridor");
    //                 DrawCells();
    //                 break;

    //                 case(1): // Is room
    //                 print("In room");
    //                 break;
    //             }

    //             }catch(MissingReferenceException e){
    //                 Debug.Log("The Taste of memes");
    //             }
    //         // }
    //         pcX2 = pcX;
    //         pcZ2 = pcZ;
    //     }else if (facing!=facing2){
    //         facing2 = facing;
    //         print("player rotated");
    //         DrawCells();
    //     }


    //     void GetDirection(){ // Get the direction the player is currently facing
    //         if (pcRot.y<45f || pcRot.y>315f){
    //             facing=0;
    //         }else if (pcRot.y>45f && pcRot.y<135f){
    //             facing=1;
    //         }else if (pcRot.y>135f && pcRot.y<225f){
    //             facing=2;
    //         }else{
    //             facing=3;
    //         }
    //     }
    // }    



    //     void DrawCells(){
    //         // List objects to show
    //         cellList.Clear();
    //         int x=pcX;
    //         int z=pcZ;
    //         bool wallFound = false;

    //         while (wallFound==false){
    //             if (WallPlacer.cellWalls[x][z][facing]==0){ // if no wall ahead of player
    //                 if (WallPlacer.cellWalls[x][z][6]==0){ // if hallway prefab
    //                     cellList.Add(WallPlacer.prefabList[x][z]);
    //                 }
    //             }else{
    //                 wallFound=true;
    //                 break;
    //             }


    //             switch(WallPlacer.cellWalls[x][z][facing]){
    //                 case(0):
    //                 x+=1;
    //                 break;
                    
    //                 case(1):
    //                 z+=1;
    //                 break;
                    
    //                 case(2):
    //                 x-=1;
    //                 break;

    //                 case(3):
    //                 z-=1;
    //                 break;
    //             }

    //             // if out of bounds
    //             if (x < 0 || x > WallPlacer.gridX || z < 0 || z > WallPlacer.gridZ){
    //                 wallFound=true;
    //                 print("had to break");
    //                 break;
    //             }
    //         }

    //         foreach(GameObject prefab in cellList){
    //             prefab.SetActive(true);
    //         }


    //         // Compare list of objects to show to objects last shown

    //         // hide objects not in the new list

    //         // overwrite list of objects last shown
    //         cellList2 = cellList;

    //         // WallPlacer.prefabList[pcX][pcZ].SetActive(true);
    //         // if (WallPlacer.cellWalls[pcX2][pcZ2][6]==0) WallPlacer.prefabList[pcX2][pcZ2].SetActive(false);
    //     }
    // }
}
