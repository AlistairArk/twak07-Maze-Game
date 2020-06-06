using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour{


    private MazeGenerator MazeGenerator;
    private MazeGlobals MazeGlobals;
    private PlayerManager PlayerManager;

    void Start() {
        // cam1 = cam1Object.GetComponent<Camera>();
        // cam2 = cam2Object.GetComponent<Camera>();
        // cam1.enabled = true;
        // cam2.enabled = false;
        MazeGenerator = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        MazeGlobals = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGlobals>();
        PlayerManager = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>();


    }




    void Update() {

    }



    public void ButtonPlayGame(){ // Default settings
        Debug.Log("PRESSED");
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 


    public void Example1(){     // Small Maze No Rooms
        MazeGlobals.mode=0;
        MazeGlobals.gridX=5;
        MazeGlobals.gridZ=5;
        MazeGlobals.gridZ=5;

        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example2(){     // Large maze large rooms
        MazeGlobals.mode=0;
        MazeGlobals.gridX=50;
        MazeGlobals.gridZ=30;
        
        MazeGenerator.GenerateSpaceStation();
        PlayerManager.MenuToGame();
    } 

    public void Example3(){     // Smalls Maze small rooms
        MazeGlobals.gridX=50;
        MazeGlobals.gridZ=50;
        MazeGlobals.type=0;
        
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


