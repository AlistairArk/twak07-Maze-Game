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


    void MenuState(){
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
