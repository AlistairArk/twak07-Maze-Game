using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : MonoBehaviour{


    [Header("Maze Settings", order=0)]
    public int gridX;
    public int gridZ;
    public int endX;
    public int endZ;
    private int startX=0;
    private int startZ=0;
    public bool showRawMaze = true;
    public bool showPrefabMaze = true;
    public bool hideBase = true;
    public bool hideWalls = true;

    // Map Prefabs
    [Header("Tile Prefabs", order=1)]
    public GameObject CorridorHall;
    public GameObject CorridorEnd;
    public GameObject CorridorCorner;
    public GameObject CorridorTJunction;
    public GameObject CorridorIntersection;
    float prefabOffsetX = .5f;
    float prefabOffsetZ = 0.5f;


    // RawMaze
    [Header("Raw Maze", order=2)]
    public GameObject wall;
    public GameObject[ ] walls;
    public Material wallMat;
    public Material playerMat;
    public GameObject goalObject;
    public GameObject playerObject;


    public List<List<List<GameObject>>> cellList = new List<List<List<GameObject>>>();
    public List<List<List<int>>> cellWalls = new List<List<List<int>>>();


    [Header("Misc.", order=3)]

    public float gridOffset;
    private float wallHeight = .3f;
    public int x;
    public int z;


    public int endDist = -1;

    public int mapScale = 10;

    int startDistance = 0;
    private CharacterController charController;
    
    public List<List<int>> stack = new List<List<int>>();
    public List<List<int>> visited = new List<List<int>>();
    public List<List<int>> optimalPath = new List<List<int>>();

    private GameObject prefabMazeParent;
    private GameObject rawMazeParent;
    private GameObject guideCubeParent;


    public void Start(){
        charController = playerObject.GetComponent<CharacterController>();

        prefabMazeParent = new GameObject("prefabMazeParent");;
        rawMazeParent = new GameObject("rawMazeParent");;
        guideCubeParent = new GameObject("guideCubeParent");;

    }
















    // Start is called before the first frame update
    public void startMap() {
        

        for(int cellX = 0; cellX < gridX; cellX++){
            List<List<GameObject>> cellRow = new List<List<GameObject>>();
            List<List<int>> cellWallsRow = new List<List<int>>();
            for(int cellZ = 0; cellZ < gridZ; cellZ++){

                List<GameObject> cell = new List<GameObject>();
                List<int> cellWallsGroup = new List<int>();

                if (showRawMaze){
                    GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale);
                    cube1.transform.Rotate(0f, 90f, 0f);
                    cube1.GetComponent<Renderer>().material = wallMat;
                    cell.Add(cube1); // North Wall

                    GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube2.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale);
                    cube2.GetComponent<Renderer>().material = wallMat;
                    cell.Add(cube2); // East Wall

                    if (hideWalls){
                        cube1.GetComponent<MeshRenderer>().enabled = false;
                        cube2.GetComponent<MeshRenderer>().enabled = false;
                    }
                    // GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // cube3.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale);
                    // cube3.transform.Rotate(0f, 90f, 0f);
                    // cube3.GetComponent<Renderer>().material = wallMat;
                    // cell.Add(cube3); // South Wall

                    // GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // cube4.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale);
                    // cube4.GetComponent<Renderer>().material = wallMat;
                    // cell.Add(cube4); // West Wall

                    // Place object under a single parent
            		cube1.transform.parent = rawMazeParent.transform;
            		cube2.transform.parent = rawMazeParent.transform;
            		// cube3.transform.parent = rawMazeParent.transform;
            		// cube4.transform.parent = rawMazeParent.transform;

            		// Set position of walls
                    cube1.transform.position = new Vector3(mapScale*(.5f+cellX),    wallHeight,  mapScale*(1f+cellZ));
                    cube2.transform.position = new Vector3(mapScale*(cellX+1f),     wallHeight,  mapScale*(cellZ+.5f));
                    // cube3.transform.position = new Vector3(mapScale*(.5f+cellX),    wallHeight,  mapScale*(cellZ));
                    // cube4.transform.position = new Vector3(mapScale*(cellX),        wallHeight,  mapScale*(cellZ+.5f));
                }
                
                // Add objects to list
                cellRow.Add(cell);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(0);  // Distance from start
                cellWallsGroup.Add(0);  // On path to goal
                cellWallsGroup.Add(0);  // Room cell
                cellWallsRow.Add(cellWallsGroup);
            }
            cellList.Add(cellRow);
            cellWalls.Add(cellWallsRow);
        }


        // if (showRawMaze){
            // Create level base
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position   = new Vector3(mapScale*(gridX*.5f), 0, mapScale*(gridZ*.5f));
            cube.transform.localScale = new Vector3(mapScale*gridX, 0.01f, mapScale*gridZ);
            cube.layer = 8;
            cube.transform.parent = rawMazeParent.transform; // Place object under a single parent
            if (hideBase) cube.GetComponent<MeshRenderer>().enabled = false;

            // South Wall
            GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube3.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridX);
            cube3.transform.Rotate(0f, 90f, 0f);
            cube3.GetComponent<Renderer>().material = wallMat;
            cube3.transform.position = new Vector3(cube3.transform.position.x+(gridX*.5f*mapScale), cube3.transform.position.y, cube3.transform.position.x);

            // West Wall
            GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube4.transform.localScale = new Vector3(.25f, 1*mapScale, 1*mapScale*gridZ);
            cube4.transform.position = new Vector3(cube4.transform.position.x, cube4.transform.position.y, cube4.transform.position.x+(gridZ*.5f*mapScale));
            cube4.GetComponent<Renderer>().material = wallMat;
        // }




        RecursiveBacktrack(startX,startZ); // Build Maze
        FindMainPath(); // Crawl through the maze and find the main path
        AddRooms(); // Interspace rooms along that path 


        float pcPosX = (startX+.5f) * mapScale;
        float pcPosZ = (startZ+.5f) * mapScale;

        // Temporarily disable controller while repositioning the player object
        charController.enabled = false;
        playerObject.transform.position = new Vector3(pcPosX, playerObject.transform.position.y, pcPosZ);
        charController.enabled = true;
    }








    public void FindMainPath(){
        // startX,startZ
        // endX, endZ
        optimalPath.Clear();
        bool endFound = false;
        int counter = 0;
        x = endX-1;
        z = endZ-1;

        int distance;

        // Get starting distance based off goal location
        print("Start Point ("+x+", "+z+")");

        distance = cellWalls[x][z][4];
        while (!endFound){
            counter++;

            // Find neighboring cell with lowest distance from start


            // If there is a adjacent path && distance is less than previous (i.e. getting closer to the start.)
            if (cellWalls[x][z][0]==0 && cellWalls[x][z+1][4]<distance){ // North
                distance=cellWalls[x][z+1][4]; z++;

            }else if (cellWalls[x][z][1]==0 && cellWalls[x+1][z][4]<distance){ // East
                distance=cellWalls[x+1][z][4]; x++;

            }else if (cellWalls[x][z][2]==0 && cellWalls[x][z-1][4]<distance){ // South
                distance=cellWalls[x][z-1][4]; z--;

            }else if (cellWalls[x][z][3]==0 && cellWalls[x-1][z][4]<distance){ // West
                distance=cellWalls[x-1][z][4]; x--;
            }

            GuideCube(x,z);
            cellWalls[x][z][5]=1;
            optimalPath.Add(new List<int>{x,z});


            if (distance==0){
                endFound = true;
            }
        }
    }




    public void GuideCube(int cubeX, int cubeZ){
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.localScale = new Vector3(1f, 1f, 1f);
        cube2.GetComponent<Renderer>().material = playerMat;
        cube2.transform.position = new Vector3((cubeX+.5f)*mapScale, 10f, (cubeZ+.5f)*mapScale);
        cube2.transform.parent = guideCubeParent.transform;

    }








    public void AddRooms(){

        // List<int> x =  new List<int>{0,1};
        // List<int> z =  new List<int>{0,1};
        List<List<int>> roomCells = new List<List<int>>();
        roomCells.Add(new List<int>{0,1});
        roomCells.Add(new List<int>{1,1});
        roomCells.Add(new List<int>{1,0});
        roomCells.Add(new List<int>{0,0});

        List<List<List<int>>> rooms = new List<List<List<int>>>();
        rooms.Add(roomCells);

        // nextCell.Add(x[i]);
        // nextCell.Add(z[i]);
        // stack.Add(nextCell);     // add to visited list
        
        // iterate over elements in optimal path
        foreach(List<int> pathElement in optimalPath){
            int X = pathElement[0];
            int Z = pathElement[1];

            foreach(List<List<int>> room in rooms){

                int cellCount = room.Count;
                foreach(List<int> cell in room){
                    int cellX = X+cell[0];
                    int cellZ = Z+cell[1];

                    // If cell is in grid and cell is on a path
                    if (cellX<gridX && cellZ<gridZ && cellWalls[cellX][cellZ][5]==1){
                        cellCount--;
                    }else{
                        break;
                    }
                    // print("( "+X+", "+Z+")   ("+cellX+", "+cellZ+")");
                }

                
                if (cellCount==0){
                    print("Room Fits: ("+X+", "+Z+")");
                    x = X;  // Set starting point
                    z = Z;
                    foreach(List<int> cell in room){
                        int cellX = X+cell[0];
                        int cellZ = Z+cell[1];
                        
                        if (cellX == x && cellZ == z+1){
                            moveN();
                        }else if (cellX == x+1 && cellZ == z){
                            moveE();
                        }else if (cellX == x && cellZ == z-1){
                            moveS();
                        }else if (cellX == x-1 && cellZ == z){
                            moveW();
                        }
                    }
                }
            }
            // print("("+X+", "+Z+")");
        }




        // for(int X = 0; X < gridX; X++){
        //     for(int Z = 0; Z < gridZ; Z++){
        //         foreach(List<List<int>> room in rooms){

        //             int cellCount = room.Count;
        //             foreach(List<int> cell in room){
        //                 int cellX = X+cell[0];
        //                 int cellZ = Z+cell[1];

        //                 // If cell is on a path
        //                 if (cellWalls[x][z][5]==1){
        //                     cellCount--;
        //                 }else{
        //                     break;
        //                 }
        //                 // print("( "+X+", "+Z+")   ("+cellX+", "+cellZ+")");
        //             }

        //             // 
        //             if (cellCount==0){
        //                 print("Room Fits: ("+X+", "+Z+")");
        //                 X = x;  // Set starting point
        //                 Z = z;
        //                 // foreach(List<int> cell in room){
        //                 //     int cellX = X+cell[0];
        //                 //     int cellZ = Z+cell[1];
                            
        //                 //     if (cellX == x && cellZ == z+1){
        //                 //         moveN();
        //                 //     }else if (cellX == x+1 && cellZ == z){
        //                 //         moveE();
        //                 //     }else if (cellX == x && cellZ == z-1){
        //                 //         moveS();
        //                 //     }else if (cellX == x-1 && cellZ == z){
        //                 //         moveW();
        //                 //     }
        //                 // }
        //             }
        //         }
        //     }
        // }
            
        // for(int i = 0; i < x.Count; i++){
        //     List<int> nextCell = new List<int>();
        //     visited.Add(nextCell);   // place current cell on to stack        
        // }

    }






    public bool availableCell(int cellX, int cellZ){
        // Debug.Log(""+cellX+">"+(gridX-1)+"   "+cellZ+">"+(gridZ-1)+"   "+cellX+"<0    "+cellZ+"<0");
        if (cellX>gridX-1 || cellZ>gridZ-1 || cellX<0 || cellZ<0) return false;
        
        for(int i = 0; i < visited.Count; i++){
            // Debug.Log("Compare: ("+ cellX + ", "+cellZ+") to ("+visited[i][0]+ ", " + visited[i][1] + ")");
            if (visited[i][0]==cellX && visited[i][1]==cellZ ){
                return false;
            }
        }
        return true;
    }
















    public void moveN(){
        cellWalls[x][z][0] = 0;
        cellWalls[x][z+1][2] = 0;
        
        if (showRawMaze){
            cellList[x][z][0].SetActive(false);
            // cellList[x][z+1][2].SetActive(false);
        }
        z+=1;
    }

    public void moveE(){
        cellWalls[x][z][1] = 0;
        cellWalls[x+1][z][3] = 0;

        if (showRawMaze){
            cellList[x][z][1].SetActive(false);
            // cellList[x+1][z][3].SetActive(false);
        }
        x+=1;
    }

    public void moveS(){
        cellWalls[x][z][2] = 0;
        cellWalls[x][z-1][0] = 0;

        if (showRawMaze){
            // cellList[x][z][2].SetActive(false);
            cellList[x][z-1][0].SetActive(false);
        }
        z-=1;
    }

    public void moveW(){
        cellWalls[x][z][3] = 0;
        cellWalls[x-1][z][1] = 0;

        if (showRawMaze){
            // cellList[x][z][3].SetActive(false);
            cellList[x-1][z][1].SetActive(false);
        }
        x-=1;                   // make this cell the current cell
    }



















    public void RecursiveBacktrack(int startX, int startZ){

        // Set start points to last end point
        startX = endX;
        startZ = endZ;
        x = endX-1;
        z = endZ-1;

        // Reset distance counters
        endDist = -1;
        startDistance = 0;

        stack.Clear();
        visited.Clear();

        print("("+x+", "+z+")");
        List<int> currentCell = new List<int>();
        currentCell.Add(x);
        currentCell.Add(z);
        // single_cell(x, z)                    // starting positing of maze

        stack.Add(currentCell);                 // place starting cell into stack
        visited.Add(currentCell);               // add starting cell to visited list


        while(stack.Count > 0){
            // Log cells relative distance from start
            // int positionFromStart = startDistance;
            cellWalls[x][z][4] = startDistance;

            List<string> cellList = new List<string>();

            if (availableCell(x,z+1)) cellList.Add("n");
            if (availableCell(x+1,z)) cellList.Add("e");
            if (availableCell(x,z-1)) cellList.Add("s");
            if (availableCell(x-1,z)) cellList.Add("w");

            // Log cells relative distance from start
            cellWalls[x][z][4] = startDistance;
            
            if (cellList.Count > 0){
                startDistance+=1;


                int cellChosen = Random.Range(0, cellList.Count);

                if (cellList[cellChosen] == "n"){
                    moveN();
                } 

                if (cellList[cellChosen] == "e"){
                    moveE();
                } 

                if (cellList[cellChosen] == "s"){
                    moveS();
                } 

                if (cellList[cellChosen] == "w"){
                    moveW();
                }
                List<int> nextCell = new List<int>();
                nextCell.Add(x);
                nextCell.Add(z);
                stack.Add(nextCell);     // add to visited list
                visited.Add(nextCell);   // place current cell on to stack

                // Debug.Log(cellList[cellChosen] + " ("+x+", "+z+")");
            }else{
                startDistance-=1;
                stack.RemoveAt(stack.Count - 1);
                if (stack.Count == 0) break;
                x = stack[stack.Count-1][0];
                z = stack[stack.Count-1][1];

            }
        }
        GetCellEnds();

    }



















    public void GetCellEnds(){
        // List all candidates for start and end points



        for(int cellX = 0; cellX < gridX; cellX++){
            for(int cellZ = 0; cellZ < gridZ; cellZ++){
                
                int findCount = 0;

                for (int i = 0; i < 4; i++){
                    if (cellWalls[cellX][cellZ][i]==0) findCount++;
                }

                if (showPrefabMaze){
                    GameObject cellPrefab;
                    switch(findCount) {
                         case (1):
                            if (cellWalls[cellX][cellZ][0] == 0 ){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                            } else if (cellWalls[cellX][cellZ][1] == 0){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                            } else if (cellWalls[cellX][cellZ][2] == 0){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            } else {
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                            }
    		                cellPrefab.transform.parent = prefabMazeParent.transform;


                            // If end cell is further than the current end cell
                            if (cellWalls[cellX][cellZ][4]>endDist  ||  endDist==-1){
                                // set new end point
                                endX = cellX+1;
                                endZ = cellZ+1;
                                endDist = cellWalls[cellX][cellZ][4];
                            }

                            break;
                        case (2):
                        	// Hallways
                            if (cellWalls[cellX][cellZ][0] == 0  && cellWalls[cellX][cellZ][2] == 0 ){
                                cellPrefab = Instantiate(CorridorHall, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                        	} else if (cellWalls[cellX][cellZ][1] == 0  && cellWalls[cellX][cellZ][3] == 0 ){
                                cellPrefab = Instantiate(CorridorHall, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));

                            // Corners
                        	} else if (cellWalls[cellX][cellZ][0] == 0  && cellWalls[cellX][cellZ][1] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                        	} else if (cellWalls[cellX][cellZ][1] == 0  && cellWalls[cellX][cellZ][2] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                        	} else if (cellWalls[cellX][cellZ][2] == 0  && cellWalls[cellX][cellZ][3] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                        	} else{
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                        	}
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                        case (3):
                        	// T Junction
                        	if (cellWalls[cellX][cellZ][0] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                        	} else if (cellWalls[cellX][cellZ][1] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                        	} else if (cellWalls[cellX][cellZ][2] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                        	} else{
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                        	}
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                        case (4):
    						cellPrefab = Instantiate(CorridorIntersection, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellZ+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                    }
                }
            }
        }
        SetSpawnPoints();

    }











    public void SetSpawnPoints(){
        float gX, gZ = 0f;
        gX  = (endX-.5f) * mapScale;
        gZ  = (endZ-.5f) * mapScale;

        // print("Goal: (" + endX + ", " + endZ + ")");
        // Place the goal object in a new location
        goalObject.transform.position = new Vector3(gX, goalObject.transform.position.y, gZ);


    }












    public void resetMap(){

        for(int cellX = 0; cellX < gridX; cellX++){
            for(int cellZ = 0; cellZ < gridZ; cellZ++){

                // if (showPrefabMaze){
                cellWalls[cellX][cellZ][0] = 1;
                cellWalls[cellX][cellZ][1] = 1;
                cellWalls[cellX][cellZ][2] = 1;
                cellWalls[cellX][cellZ][3] = 1;
                cellWalls[cellX][cellZ][4] = 0;
                cellWalls[cellX][cellZ][5] = 0;
                cellWalls[cellX][cellZ][6] = 0;
                // }

                if (showRawMaze){
                    cellList[cellX][cellZ][0].SetActive(true);
                    cellList[cellX][cellZ][1].SetActive(true);
                    // cellList[cellX][cellZ][2].SetActive(true);
                    // cellList[cellX][cellZ][3].SetActive(true);
                }
            }
        }



        foreach (Transform child in prefabMazeParent.transform) {
            GameObject.Destroy(child.gameObject);
        }


        RecursiveBacktrack(startX,startZ); // Build Maze
    } 
}


