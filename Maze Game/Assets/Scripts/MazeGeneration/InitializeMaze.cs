using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMaze : MonoBehaviour {

    public MazeGlobals MazeGlobals;

    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
    }



    // Start is called before the first frame update
    public void Initialize() {

        if (MazeGlobals.mode==0) MazeGlobals.cellList = new List<List<List<GameObject>>>();     
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;
        int mapScale = MazeGlobals.mapScale;
        bool hideWalls = MazeGlobals.hideWalls;
        bool showRawMaze = MazeGlobals.showRawMaze;
        Material whiteMat = MazeGlobals.whiteMat;
        Material wallMat = MazeGlobals.wallMat;
        float wallHeight = MazeGlobals.wallHeight;
        GameObject rawMazeParent = MazeGlobals.rawMazeParent;
        GameObject cellBaseParent = MazeGlobals.cellBaseParent;
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();


        for(int cellX = 0; cellX < gridX; cellX++){

            // List<List<GameObject>> cellRow = new List<List<GameObject>>();
            List<List<int>> cellDataRow = new List<List<int>>();

            for(int cellZ = 0; cellZ < gridZ; cellZ++){

                List<int> cellDataGroup = new List<int>();
                List<GameObject> cell = new List<GameObject>();
                
                // Add objects to list
                cellDataGroup.Add(1);  // 0 - N (=1 if the cell has a North wall)
                cellDataGroup.Add(1);  // 1 - E (=1 if the cell has a East wall)
                cellDataGroup.Add(1);  // 2 - S (=1 if the cell has a South wall)
                cellDataGroup.Add(1);  // 3 - W (=1 if the cell has a West wall)
                cellDataGroup.Add(0);  // 4 - Distance from start
                cellDataGroup.Add(0);  // 5 - On path to goal (=1 if cell is on path to the goal) (=0 if cell is not on path to the goal)
                cellDataGroup.Add(0);  // 6 - (=1 if room cell) (=0 if corridor cell)
                cellDataGroup.Add(0);  // 7 - Cell UID (=0 if corridor cell) (=+1 room cells - groups of cells for each room are assigned a UID)
                cellDataRow.Add(cellDataGroup);
            }
            cellData.Add(cellDataRow);
        }


        // Create level base
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position   = new Vector3(mapScale*(gridX*.5f), 0, mapScale*(gridZ*.5f));
        ground.transform.localScale = new Vector3(mapScale*gridX, 0.01f, mapScale*gridZ);
        ground.layer = 8;
        ground.transform.parent = rawMazeParent.transform; // Place object under a single parent
        ground.GetComponent<MeshRenderer>().enabled = false;
        ground.transform.parent = cellBaseParent.transform; // Place object under a single parent
        ground.GetComponent<Collider>().isTrigger = true;

        GameObject mapGround = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mapGround.transform.position   = new Vector3(mapScale*(gridX*.5f), -4f, mapScale*(gridZ*.5f));
        mapGround.transform.localScale = new Vector3(mapScale*gridX, 0.01f, mapScale*gridZ);
        mapGround.GetComponent<Renderer>().material = whiteMat;
        mapGround.layer = 8;
        mapGround.transform.parent = cellBaseParent.transform; // Place object under a single parent

        // // South Wall
        // GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube3.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridX);
        // cube3.transform.Rotate(0f, 90f, 0f);
        // cube3.GetComponent<Renderer>().material = wallMat;
        // cube3.transform.position = new Vector3(cube3.transform.position.x+(gridX*.5f*mapScale), cube3.transform.position.y, cube3.transform.position.x);
        // cube3.transform.parent = cellBaseParent.transform; // Place object under a single parent
        // cube3.layer = 10;

        // // West Wall
        // GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube4.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridZ);
        // cube4.transform.position = new Vector3(cube4.transform.position.x, cube4.transform.position.y, cube4.transform.position.x+(gridZ*.5f*mapScale));
        // cube4.GetComponent<Renderer>().material = wallMat;
        // cube4.transform.parent = cellBaseParent.transform; // Place object under a single parent
        // cube4.layer = 10;


    }
}
