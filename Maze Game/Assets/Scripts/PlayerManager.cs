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


    [Header("Player Attributes", order=0)]
    public bool pauseTimer = true;      // Timer is paused while in hacker game
    public float timeTaken = 0;         // Time taken to clear the maze  
    public int cellsTravelled = 0;      // Distance travelled (measured in number of maze cells)  
    public int playerX = 0; public int playerY = 0;     // Log player co-ords for cell movement

    // Start is called before the first frame update
    void Awake(){
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
        if (!pauseTimer) timeTaken+=0.02f;
    }

    public void ResetMetrics(){
        /*
        Reset metric parameters and calculate parameters for the next maze generaton
        */

        timeTaken = 0f;
        cellsTravelled = 0;

        // Check how player peformed in the last maze

        // Check how the player peformed in the current maze

        // If the player peforms better than last time create a harder maze

        // If the player peforms worse, create an easier maze

        // If player peforms about the same, create the same maze.



    }


    void MenuState(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

        if (enableVR){
            stationCamVR.SetActive(true);
            hackerCamVR.SetActive(false);
        }else{
            stationCamStandard.SetActive(true);
            hackerCamStandard.SetActive(false);
        }
    }
}
