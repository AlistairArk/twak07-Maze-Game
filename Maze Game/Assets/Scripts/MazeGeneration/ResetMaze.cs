using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMaze : MonoBehaviour{


    public MazeGlobals MazeGlobals;

    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
    }
    

    void Reset(){
        for(int X = 0; X < MazeGlobals.gridX; X++){
            for(int Z = 0; Z < MazeGlobals.gridZ; Z++){

                MazeGlobals.cellData[X][Z][0] = 1;
                MazeGlobals.cellData[X][Z][1] = 1;
                MazeGlobals.cellData[X][Z][2] = 1;
                MazeGlobals.cellData[X][Z][3] = 1;
                MazeGlobals.cellData[X][Z][4] = 0;
                MazeGlobals.cellData[X][Z][5] = 0;
                MazeGlobals.cellData[X][Z][6] = 0;

                if (MazeGlobals.showRawMaze){
                    MazeGlobals.cellList[X][Z][0].SetActive(true);
                    MazeGlobals.cellList[X][Z][1].SetActive(true);
                }
            }
        }


        /* You can deactive the container by using SetActive(false); instead of destroying it. And then run a coroutine to destroy every child object every frame. This might speed things up. By the way it is always good to show a loading screen while swithing between levels. */
        foreach (Transform child in MazeGlobals.rawMazeParent.transform) child.gameObject.SetActive(true);
        foreach (Transform child in MazeGlobals.guideCubeParent.transform) GameObject.Destroy(child.gameObject);
        foreach (Transform child in MazeGlobals.prefabMazeParent.transform) GameObject.Destroy(child.gameObject);
        foreach (Transform child in MazeGlobals.cellDoorParent.transform) GameObject.Destroy(child.gameObject);
        foreach (Transform child in MazeGlobals.cellWallParent.transform) GameObject.Destroy(child.gameObject);


        // RecursiveBacktrack(startX,startZ); // Build Maze
        // FindMainPath();     // Crawl through the maze and find the main path
        // AddRooms();         // Interspace rooms along that path 
        // HidePrefabs();   
    }
}
