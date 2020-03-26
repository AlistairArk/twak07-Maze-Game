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


            /* You can deactive the container by using SetActive(false); instead of destroying it. And then run a coroutine to destroy every child object every frame. This might speed things up. By the way it is always good to show a loading screen while swithing between levels. */
            foreach (Transform child in MazeGlobals.rawMazeParent.transform) child.gameObject.SetActive(true);
            foreach (Transform child in MazeGlobals.guideCubeParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.prefabMazeParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.cellDoorParent.transform) GameObject.Destroy(child.gameObject);
            foreach (Transform child in MazeGlobals.cellWallParent.transform) GameObject.Destroy(child.gameObject);
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
            // MazeGlobals.cellDataHack = new List<List<List<int>>>();
            // int gridX = MazeGlobals.gridX;
            // int gridZ = MazeGlobals.gridZ;
            // print("(X "+gridX+",  Z "+gridZ+")");
            // int counter = 0;
            // int X=-1;
            // foreach( List<List<int>> column in cellData ){
            //     int Z=-1;
            //     counter++;
            //     X++;
            //     foreach( List<int> row in column ){
            //         Z++;
            //         print("X "+counter+"   "+ row[0]+"  "+X+"  "+Z);
            //         for (int i = 0; i < 4; i++){
            //             if (cellData[X][Z][i]==0){
                            
            //             }
            //             print("X "+gridX+"  Z "+gridZ + "   " + cellData[X][Z][i]);
            //         }
            //     }
            // }
            // print("counter " + counter);
            // // for(int X = 0; X < gridX; X++){
            // //     for(int Z = 0; Z < gridZ; Z++){
            // //         int findCount = 0;
            // //         for (int i = 0; i < 4; i++){
            // //             if (cellData[X][Z][i]==0) findCount++;
            // //             print("X "+gridX+"  Z "+gridZ + "   " + cellData[X][Z][i]);
            // //         }
            // //     }
            // // }




            // for(int X = 0; X < MazeGlobals.gridX; X++){
            //     for(int Z = 0; Z < MazeGlobals.gridZ; Z++){

            //         cellData[X][Z][0] = 1;
            //         cellData[X][Z][1] = 1;
            //         cellData[X][Z][2] = 1;
            //         cellData[X][Z][3] = 1;
            //         cellData[X][Z][4] = 0;
            //         cellData[X][Z][5] = 0;
            //         cellData[X][Z][6] = 0;
            //         cellData[X][Z][7] = 0;

            //         if (MazeGlobals.showRawMaze){
            //             MazeGlobals.cellList[X][Z][0].SetActive(true);
            //             MazeGlobals.cellList[X][Z][1].SetActive(true);
            //         }
            //     }
            // }

            // for(int X = 0; X < MazeGlobals.gridX; X++){
            //     for(int Z = 0; Z < MazeGlobals.gridZ; Z++){

            //         cellData[X][Z][0] = 1;
            //         cellData[X][Z][1] = 1;
            //         cellData[X][Z][2] = 1;
            //         cellData[X][Z][3] = 1;
            //         cellData[X][Z][4] = 0;
            //         cellData[X][Z][5] = 0;
            //         cellData[X][Z][6] = 0;
            //         cellData[X][Z][7] = 0;



                    
            //     }
            // }