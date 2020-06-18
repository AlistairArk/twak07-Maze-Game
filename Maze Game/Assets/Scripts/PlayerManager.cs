using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{

    [Header("Player Settings", order=0)]
    public bool enableVR = false;

    [Header("Cameras", order=1)]
    public GameObject menuCamStandard;
    public GameObject menuCamVR;
    public GameObject stationCamStandard;
    public GameObject stationCamVR;   
    public GameObject hackerCamStandard;
    public GameObject hackerCamVR;   
    public GameObject orbitalCam;   

    [Header("Object Groups", order=2)]
    public GameObject menuGroup;
    public GameObject stationGroup;   
    public GameObject hackingGroup;   


    [Header("Misc.", order=3)]
    public string gameState = "menu";
    public MazeGlobals MazeGlobals;


    [Header("Player Attributes", order=0)]
    public bool pauseTimer = true;      // Timer is paused while in hacker game
    public float timeTaken = 0;         // Time taken to clear the maze  
    public float timeTakenL = 0;        // Time taken last maze
    public int cellsTravelled = 0;      // Distance travelled (measured in number of maze cells)  
    public int playerX = 0; public int playerZ = 0;     // Log player co-ords for cell movement
    public float doorLockChance = 0f;

    // Start is called before the first frame update
    void Awake(){
        MazeGlobals = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGlobals>();
        
        menuGroup    = GameObject.FindWithTag("MenuGroup");
        stationGroup = GameObject.FindWithTag("StationGroup");
        hackingGroup = GameObject.FindWithTag("HackingGroup");

        menuCamStandard    = GameObject.FindWithTag("MenuCamStandard");
        menuCamVR          = GameObject.FindWithTag("MenuCamVR");
        stationCamStandard = GameObject.FindWithTag("StationCamStandard");
        stationCamVR       = GameObject.FindWithTag("StationCamVR");
        hackerCamStandard  = GameObject.FindWithTag("HackerCamStandard");
        hackerCamVR        = GameObject.FindWithTag("HackerCamVR");
        orbitalCam         = GameObject.FindWithTag("OrbitalCam");


        MenuState();
    }

    void FixedUpdate(){
        // Only record player time while the pause timer is set to false
        if (!pauseTimer){
            timeTaken+=0.02f;
            
            // if player moves to new cell
            if (playerX!=(int)stationCamStandard.transform.position.x/10 || playerZ!=(int)stationCamStandard.transform.position.z/10){
                playerX = (int)stationCamStandard.transform.position.x/10;
                playerZ = (int)stationCamStandard.transform.position.z/10;
                cellsTravelled++;              
            }
        }
    }

    public void ResetMetrics(){
        /*
        Reset metric parameters and calculate parameters for the next maze generaton
        */
        float averageSpeed = 10f;
        int changeDif = 0;

        print("=====================================");
        print(MazeGlobals.startDistance);
        print(MazeGlobals.endX);
        print(MazeGlobals.endZ);

        print(MazeGlobals.endDist); // end cells relative distance from start


        // Check how player peformed in the last maze

        // Check how the player peformed in the current maze
        // Distance Check
        if (cellsTravelled < (MazeGlobals.endDist*=0.80)){
            changeDif++;
        }else if (cellsTravelled > (MazeGlobals.endDist*=1.20)){
            changeDif--;
        }else{
            // No changes to difficulty
        }

        // Time Check
        if (timeTakenL < (timeTaken*=0.80)){
            changeDif++;
        }else if (timeTakenL > (timeTaken*=1.20)){
            changeDif--;
        }else{
            // No changes to difficulty
        }


        // If the player peforms better than last time create a harder maze
        switch(changeDif){
            case(-2): // Decrease locked door and size
            doorLockChance-=.1f;
            break;

            case(-1): // Decrease size
            MazeGlobals.gridX-=1;
            MazeGlobals.gridZ-=1;
            break;

            case(1): // Increase size
            MazeGlobals.gridX+=1;
            MazeGlobals.gridZ+=1;
            break;

            case(2): // Increase locked door and size
            doorLockChance+=.1f;
            break;
        }

        // If the player peforms worse, create an easier maze

        // If player peforms about the same, create the same maze.


        print("==========RESETTING METRICS==========");
        timeTaken = 0f;
        cellsTravelled = 0;

    }


    void MenuState(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseTimer = true;

        menuGroup.SetActive(true);
        stationGroup.SetActive(false);
        hackingGroup.SetActive(false);

        if (enableVR){
            menuCamVR.SetActive(true);
            menuCamStandard.SetActive(false);
        }else{
            menuCamVR.SetActive(false);
            menuCamStandard.SetActive(true);
        }

        stationCamStandard.SetActive(false);
        stationCamVR.SetActive(false);
        hackerCamStandard.SetActive(false);
        hackerCamVR.SetActive(false);

        orbitalCam.SetActive(false);
    }



    public void MenuToGame(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseTimer = false;

        orbitalCam.SetActive(true);
        menuGroup.SetActive(false);
        stationGroup.SetActive(true);

        if (enableVR){
            menuCamVR.SetActive(false);
            stationCamVR.SetActive(true);
        }else{
            menuCamStandard.SetActive(false);
            stationCamStandard.SetActive(true);
        }
    }

    public void GameToMenu(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseTimer = true;

        menuGroup.SetActive(true);
        orbitalCam.SetActive(false);
        stationGroup.SetActive(false);

        if (enableVR){
            menuCamVR.SetActive(true);
            stationCamVR.SetActive(false);
        }else{
            menuCamStandard.SetActive(true);
            stationCamStandard.SetActive(false);
        }
    }


    public void GameToHack(){
        stationGroup.SetActive(false);
        hackingGroup.SetActive(true);
        orbitalCam.SetActive(false);
        pauseTimer = true;

        if (enableVR){
            stationCamVR.SetActive(false);
            hackerCamVR.SetActive(true);
        }else{
            stationCamStandard.SetActive(false);
            hackerCamStandard.SetActive(true);
        }
    }



    public void HackToGame(){
        stationGroup.SetActive(true);
        hackingGroup.SetActive(false);
        orbitalCam.SetActive(true);
        pauseTimer = false;

        if (enableVR){
            stationCamVR.SetActive(true);
            hackerCamVR.SetActive(false);
        }else{
            stationCamStandard.SetActive(true);
            hackerCamStandard.SetActive(false);
        }
    }
}
