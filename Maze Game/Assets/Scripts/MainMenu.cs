using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour{

    [Header("Menu", order=0)]

    public GameObject menuUI;
    public GameObject cam2Object;
    private Camera cam2;


    [Header("In Game", order=1)]
    public GameObject cam1Object;
    public GameObject gameUI;
    private Camera cam1;

    [Header("Misc.", order=2)]
    public bool showMenu = true;

    private MazeGenerator MazeGenerator;
    private MazeGlobals MazeGlobals;
    private PlayerManager PlayerManager;

    void Start() {
        // cam1 = cam1Object.GetComponent<Camera>();
        // cam2 = cam2Object.GetComponent<Camera>();
        // cam1.enabled = true;
        // cam2.enabled = false;
        // MazeGenerator = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        // MazeGlobals = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGlobals>();
        PlayerManager = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>();
    }




    void Update() {
        // if (MazeGenerator.gameStatus==0){   // If the player is not hacking
        //     switch(showMenu){
        //         case(true): // Menu
        //         cam2.enabled = true;
        //         cam1.enabled = false;
        //         gameUI.SetActive(false);
        //         menuUI.SetActive(true);
        //         Cursor.lockState = CursorLockMode.None;
        //         break;

        //         case(false): // Game
        //         cam1.enabled = true;
        //         cam2.enabled = false;
        //         gameUI.SetActive(true);
        //         menuUI.SetActive(false);
        //         Cursor.lockState = CursorLockMode.Locked;
        //         break;
        //     }

        //     // if (Input.GetKeyDown(KeyCode.Escape)) {
        //     //     showMenu =!showMenu;
        //     // }
        // }
    }



    // public void ButtonPlayGame(){
    //     // showMenu =!showMenu;
        
    // } 


    public void Example1(){     // Small Maze No Rooms
        MazeGlobals.mode=0;
        MazeGlobals.gridX=5;
        MazeGlobals.gridZ=5;

        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example2(){     // Large maze large rooms
        MazeGlobals.mode=0;
        MazeGlobals.gridX=10;
        MazeGlobals.gridZ=10;
        
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example3(){     // Smalls Maze small rooms
        MazeGlobals.gridX=10;
        MazeGlobals.gridZ=10;
        
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example4(){     // large Maze mixed rooms
        MazeGlobals.gridX=20;
        MazeGlobals.gridZ=20;
        
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example5(){     // large Maze mixed rooms
        MazeGlobals.gridX=20;
        MazeGlobals.gridZ=20;
        
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 
}


