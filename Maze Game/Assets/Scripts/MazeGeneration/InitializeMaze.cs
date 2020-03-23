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
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;
        int mapScale = MazeGlobals.mapScale;
        bool hideWalls = MazeGlobals.hideWalls;
        bool showRawMaze = MazeGlobals.showRawMaze;
        Material whiteMat = MazeGlobals.whiteMat;
        Material wallMat = MazeGlobals.wallMat;
        float wallHeight = MazeGlobals.wallHeight;
        GameObject rawMazeParent = MazeGlobals.rawMazeParent;
        GameObject mapObjects = MazeGlobals.mapObjects;

        for(int cellX = 0; cellX < gridX; cellX++){

            List<List<GameObject>> cellRow = new List<List<GameObject>>();
            List<List<int>> cellDataRow = new List<List<int>>();

            for(int cellZ = 0; cellZ < gridZ; cellZ++){

                List<GameObject> cell = new List<GameObject>();
                List<int> cellDataGroup = new List<int>();

                if (showRawMaze){
                    GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.localScale = new Vector3(.25f, 2*mapScale, 1*mapScale);
                    cube1.transform.Rotate(0f, 90f, 0f);
                    cube1.GetComponent<Renderer>().material = wallMat;
                    cube1.AddComponent<ObjectHider>();
                    cube1.tag = "Occludable";
                    cell.Add(cube1); // North Wall

                    GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube2.transform.localScale = new Vector3(.25f, 2*mapScale, 1*mapScale);
                    cube2.GetComponent<Renderer>().material = wallMat;
                    cube2.AddComponent<ObjectHider>();
                    cube2.tag = "Occludable";
                    cell.Add(cube2); // East Wall

                    if (hideWalls){
                        cube1.GetComponent<MeshRenderer>().enabled = false;
                        cube2.GetComponent<MeshRenderer>().enabled = false;
                    }

                    // Place object under a single parent
                    cube1.transform.parent = rawMazeParent.transform;
                    cube2.transform.parent = rawMazeParent.transform;

                    // Set position of walls
                    cube1.transform.position = new Vector3(mapScale*(.5f+cellX),    wallHeight,  mapScale*(1f+cellZ));
                    cube2.transform.position = new Vector3(mapScale*(cellX+1f),     wallHeight,  mapScale*(cellZ+.5f));
                }
                
                // Add objects to list
                cellRow.Add(cell);
                cellDataGroup.Add(1);  // N
                cellDataGroup.Add(1);  // E
                cellDataGroup.Add(1);  // S
                cellDataGroup.Add(1);  // W 
                cellDataGroup.Add(0);  // Distance from start
                cellDataGroup.Add(0);  // On path to goal
                cellDataGroup.Add(0);  // Room cell
                cellDataGroup.Add(0);  // RoomCellID
                cellDataRow.Add(cellDataGroup);
            }
            MazeGlobals.cellList.Add(cellRow);
            MazeGlobals.cellData.Add(cellDataRow);
        }


        // Create level base
        // GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // ground.transform.position   = new Vector3(mapScale*(gridX*.5f), 0, mapScale*(gridZ*.5f));
        // ground.transform.localScale = new Vector3(mapScale*gridX, 0.01f, mapScale*gridZ);
        // ground.layer = 8;
        // ground.transform.parent = rawMazeParent.transform; // Place object under a single parent
        // if (hideBase) ground.GetComponent<MeshRenderer>().enabled = false;
        // ground.transform.parent = mapObjects.transform; // Place object under a single parent

        GameObject mapGround = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mapGround.transform.position   = new Vector3(mapScale*(gridX*.5f), -4f, mapScale*(gridZ*.5f));
        mapGround.transform.localScale = new Vector3(mapScale*gridX, 0.01f, mapScale*gridZ);
        mapGround.GetComponent<Renderer>().material = whiteMat;
        mapGround.layer = 8;
        mapGround.transform.parent = mapObjects.transform; // Place object under a single parent

        // South Wall
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube3.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridX);
        cube3.transform.Rotate(0f, 90f, 0f);
        cube3.GetComponent<Renderer>().material = wallMat;
        cube3.transform.position = new Vector3(cube3.transform.position.x+(gridX*.5f*mapScale), cube3.transform.position.y, cube3.transform.position.x);
        cube3.transform.parent = mapObjects.transform; // Place object under a single parent

        // West Wall
        GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube4.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridZ);
        cube4.transform.position = new Vector3(cube4.transform.position.x, cube4.transform.position.y, cube4.transform.position.x+(gridZ*.5f*mapScale));
        cube4.GetComponent<Renderer>().material = wallMat;
        cube4.transform.parent = mapObjects.transform; // Place object under a single parent


    }
}
