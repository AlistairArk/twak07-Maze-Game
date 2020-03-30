using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMaze : MonoBehaviour{



    public MazeGlobals MazeGlobals;
    public InitializeMaze InitializeMaze;

    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
        InitializeMaze = gameObject.GetComponent<InitializeMaze>();
    }
    

    public void Reset(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        // Set maze prefrences back to their defaults
        if (MazeGlobals.mode==0){

            if (MazeGlobals.endX>MazeGlobals.gridX || MazeGlobals.endZ>MazeGlobals.gridZ){
                // RESET END X/Z ???
                MazeGlobals.endX = MazeGlobals.gridX;
                MazeGlobals.endZ = MazeGlobals.gridZ;
            }

            /* You can deactive the container by using SetActive(false); instead of destroying it. And then run a coroutine to destroy every child object every frame. This might speed things up. By the way it is always good to show a loading screen while swithing between levels. */
            foreach (Transform child in MazeGlobals.rawMazeParent.transform) child.gameObject.SetActive(true);
            foreach (Transform child in MazeGlobals.guideCubeParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.prefabMazeParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.cellDoorParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.cellWallParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.cellBaseParent.transform) GameObject.Destroy(child.gameObject);

            MazeGlobals.cellData = new List<List<List<int>>>();
            InitializeMaze.Initialize();


        }else if(MazeGlobals.mode==1){
            // RESET END X/Z ???
            MazeGlobals.endX = MazeGlobals.gridX;
            MazeGlobals.endZ = MazeGlobals.gridZ;

            foreach (Transform child in MazeGlobals.prefabHackParent.transform) GameObject.Destroy(child.gameObject);
            InitializeMaze.Initialize();

            cellData = MazeGlobals.GetCellData();

        }
    }
}

