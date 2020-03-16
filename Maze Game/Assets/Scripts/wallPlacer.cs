using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : MonoBehaviour{


    [Header("Maze Settings", order=0)]
    public int gridX;
    public int gridZ;
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
    float prefabOffsetY = 0.5f;
    public GameObject prefabMazeParent;


    // RawMaze
    [Header("Raw Maze", order=2)]
    public GameObject wall;
    public GameObject[ ] walls;
    public Material wallMat;
    public GameObject goalObject;
    public GameObject playerObject;

    public GameObject rawMazeParent;
    public List<List<List<GameObject>>> cellList = new List<List<List<GameObject>>>();
    public List<List<List<int>>> cellWalls = new List<List<List<int>>>();


    [Header("Misc.", order=3)]

    public float gridOffset;
    private float wallHeight = .3f;
    public int x;
    public int y;

    public int startX;
    public int startY;
    public int startDist = -1;
    public int endX;
    public int endY;
    public int endDist = -1;

    public int mapScale = 10;

    int startDistance = 0;
    private CharacterController charController;
    
    public List<List<int>> stack = new List<List<int>>();
    public List<List<int>> visited = new List<List<int>>();

    public void Start(){
        charController = playerObject.GetComponent<CharacterController>();

    }



    // Start is called before the first frame update
    public void startMap() {
        

        for(int cellX = 0; cellX < gridX; cellX++){
            List<List<GameObject>> cellRow = new List<List<GameObject>>();
            List<List<int>> cellWallsRow = new List<List<int>>();
            for(int cellY = 0; cellY < gridZ; cellY++){

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
                    cube1.transform.position = new Vector3(mapScale*(.5f+cellX),    wallHeight,  mapScale*(1f+cellY));
                    cube2.transform.position = new Vector3(mapScale*(cellX+1f),     wallHeight,  mapScale*(cellY+.5f));
                    // cube3.transform.position = new Vector3(mapScale*(.5f+cellX),    wallHeight,  mapScale*(cellY));
                    // cube4.transform.position = new Vector3(mapScale*(cellX),        wallHeight,  mapScale*(cellY+.5f));
                }
                
                // Add objects to list
                cellRow.Add(cell);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(1);
                cellWallsGroup.Add(0);
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

        AddRooms();
        print(visited);
        recursiveBacktrack(gridX-1,gridZ-1); // Build Maze
    }


    public void AddRooms(){

        List<int> x =  new List<int>{1,1,2,2};
        List<int> y =  new List<int>{1,2,2,1};
        
        for(int i = 0; i < x.Count; i++){
            List<int> nextCell = new List<int>();
            nextCell.Add(x[i]);
            nextCell.Add(y[i]);
            print("("+x[i]+", "+y[i]+")");
            stack.Add(nextCell);     // add to visited list
            visited.Add(nextCell);   // place current cell on to stack        
        }

    }

    public bool availableCell(int cellX, int cellY){
        // Debug.Log(""+cellX+">"+(gridX-1)+"   "+cellY+">"+(gridZ-1)+"   "+cellX+"<0    "+cellY+"<0");
        if (cellX>gridX-1 || cellY>gridZ-1 || cellX<0 || cellY<0) return false;
        for(int i = 0; i < visited.Count; i++){
            // Debug.Log("Compare: ("+ cellX + ", "+cellY+") to ("+visited[i][0]+ ", " + visited[i][1] + ")");
            if (visited[i][0]==cellX && visited[i][1]==cellY ){
                return false;
            }
        }
        return true;
    }



    public void moveN(){
        cellWalls[x][y][0] = 0;
        cellWalls[x][y+1][2] = 0;
        
        if (showRawMaze){
            cellList[x][y][0].SetActive(false);
            // cellList[x][y+1][2].SetActive(false);
        }
    }

    public void moveE(){
        cellWalls[x][y][1] = 0;
        cellWalls[x+1][y][3] = 0;

        if (showRawMaze){
            cellList[x][y][1].SetActive(false);
            // cellList[x+1][y][3].SetActive(false);
        }
    }

    public void moveS(){
        cellWalls[x][y][2] = 0;
        cellWalls[x][y-1][0] = 0;

        if (showRawMaze){
            // cellList[x][y][2].SetActive(false);
            cellList[x][y-1][0].SetActive(false);
        }
    }

    public void moveW(){
        cellWalls[x][y][3] = 0;
        cellWalls[x-1][y][1] = 0;

        if (showRawMaze){
            // cellList[x][y][3].SetActive(false);
            cellList[x-1][y][1].SetActive(false);
        }
    }



    public void recursiveBacktrack(int startX, int startY){

        x = startX;
        y = startY;

        List<int> currentCell = new List<int>();
        currentCell.Add(x);
        currentCell.Add(y);
        // single_cell(x, y)                    // starting positing of maze

        stack.Add(currentCell);                 // place starting cell into stack
        visited.Add(currentCell);               // add starting cell to visited list


        while(stack.Count > 0){
        // for(int m = 0; m < 50; m++){
            List<string> cellList = new List<string>();

            if (availableCell(x,y+1)) cellList.Add("n");
            if (availableCell(x+1,y)) cellList.Add("e");
            if (availableCell(x,y-1)) cellList.Add("s");
            if (availableCell(x-1,y)) cellList.Add("w");

            if (cellList.Count > 0){
                startDistance+=1;
                cellWalls[x][y][4] = startDistance;

                // for(int i = 0; i < cellList.Count; i++) Debug.Log(cellList[i]);

                int cellChosen = Random.Range(0, cellList.Count);

                if (cellList[cellChosen] == "n"){
                    moveN();
                    y+=1;
                } 

                if (cellList[cellChosen] == "e"){
                    moveE();
                    x+=1;
                } 

                if (cellList[cellChosen] == "s"){
                    moveS();
                    y-=1;
                } 

                if (cellList[cellChosen] == "w"){
                    moveW();
                    x-=1;                   // make this cell the current cell
                }
                List<int> nextCell = new List<int>();
                nextCell.Add(x);
                nextCell.Add(y);
                stack.Add(nextCell);     // add to visited list
                visited.Add(nextCell);   // place current cell on to stack

                // Debug.Log(cellList[cellChosen] + " ("+x+", "+y+")");
            }else{
                startDistance-=1;
                stack.RemoveAt(stack.Count - 1);
                if (stack.Count == 0) break;
                x = stack[stack.Count-1][0];
                y = stack[stack.Count-1][1];

            }
        }
        getCellEnds();
    }




    public void getCellEnds(){
        // List all candidates for start and end points


        for(int cellX = 0; cellX < gridX; cellX++){
            for(int cellY = 0; cellY < gridZ; cellY++){
                
                int findCount = 0;
                // cell is a end point if it has only 1 opening
                for (int i = 0; i < 4; i++){
                    if (cellWalls[cellX][cellY][i]==0) findCount++;
                }

                if (findCount==1){
                    if (cellWalls[cellX][cellY][4]<startDist||startDist==-1){
                        startX = cellX+1;
                        startY = cellY+1;
                        startDist = cellWalls[cellX][cellY][4];
                    }else if (cellWalls[cellX][cellY][4]>endDist||endDist==-1){
                        endX = cellX;
                        endY = cellY;
                        endDist = cellWalls[cellX][cellY][4];
                    }
                }


        		
                if (showPrefabMaze){
                    GameObject cellPrefab;
                    switch(findCount) {
                         case (1):
                            if (cellWalls[cellX][cellY][0] == 0 ){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,180,0));
                            } else if (cellWalls[cellX][cellY][1] == 0){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,270,0));
                            } else if (cellWalls[cellX][cellY][2] == 0){
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,0,0));
                            } else {
                                cellPrefab = Instantiate(CorridorEnd, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,90,0));
                            }
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                        case (2):
                        	// Hallways
                            if (cellWalls[cellX][cellY][0] == 0  && cellWalls[cellX][cellY][2] == 0 ){
                                cellPrefab = Instantiate(CorridorHall, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,0,0));
                        	} else if (cellWalls[cellX][cellY][1] == 0  && cellWalls[cellX][cellY][3] == 0 ){
                                cellPrefab = Instantiate(CorridorHall, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,90,0));

                            // Corners
                        	} else if (cellWalls[cellX][cellY][0] == 0  && cellWalls[cellX][cellY][1] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,270,0));
                        	} else if (cellWalls[cellX][cellY][1] == 0  && cellWalls[cellX][cellY][2] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,0,0));
                        	} else if (cellWalls[cellX][cellY][2] == 0  && cellWalls[cellX][cellY][3] == 0 ){
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,90,0));
                        	} else{
                                cellPrefab = Instantiate(CorridorCorner, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,180,0));
                        	}
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                        case (3):
                        	// T Junction
                        	if (cellWalls[cellX][cellY][0] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,90,0));
                        	} else if (cellWalls[cellX][cellY][1] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,180,0));
                        	} else if (cellWalls[cellX][cellY][2] == 1){
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,270,0));
                        	} else{
                                cellPrefab = Instantiate(CorridorTJunction, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,0,0));
                        	}
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                        case (4):
    						cellPrefab = Instantiate(CorridorIntersection, new Vector3((cellX+prefabOffsetX)*mapScale, 0, (cellY+prefabOffsetY)*mapScale), Quaternion.Euler(0,0,0));
    		                cellPrefab.transform.parent = prefabMazeParent.transform;
                            break;
                    }
                }
            }
        }
        setSpawnPoints();
    }


    public void setSpawnPoints(){
        float gX, gY, sX, sY = 0f;
        gX  = (endX-.5f) * mapScale;
        gY  = (endY-.5f) * mapScale;
        sX  = (startX-.5f) * mapScale;
        sY  = (startY-.5f) * mapScale;

        print("Goal: ("+gX+", "+gY+")   Player: ("+sX+", "+sY+")");
        
        // Place the goal object in a new location
        goalObject.transform.position = new Vector3(gX, goalObject.transform.position.y, gY);

        // Temporarily disable controller while repositioning the player object
        charController.enabled = false;
        playerObject.transform.position = new Vector3(sX, playerObject.transform.position.y, sY);
        charController.enabled = true;
    }

    public void resetMap(){

        for(int cellX = 0; cellX < gridX; cellX++){
            for(int cellY = 0; cellY < gridZ; cellY++){

                // if (showPrefabMaze){
                cellWalls[cellX][cellY][0] = 1;
                cellWalls[cellX][cellY][1] = 1;
                cellWalls[cellX][cellY][2] = 1;
                cellWalls[cellX][cellY][3] = 1;
                cellWalls[cellX][cellY][4] = 0;
                // }

                if (showRawMaze){
                    cellList[cellX][cellY][0].SetActive(true);
                    cellList[cellX][cellY][1].SetActive(true);
                    // cellList[cellX][cellY][2].SetActive(true);
                    // cellList[cellX][cellY][3].SetActive(true);
                }
            }
        }

        startX=0;
        startY=0;
        endX=0;
        endY=0;
        startDist = -1;
        endDist = -1;
        startDistance = 0;
        stack.Clear();
        visited.Clear();

        foreach (Transform child in prefabMazeParent.transform) {
            GameObject.Destroy(child.gameObject);
        }

        recursiveBacktrack(gridX-1,gridZ-1); // Build Maze
        goalObject.transform.position = new Vector3(endX, goalObject.transform.position.y, endY-0.5f);
        playerObject.transform.position = new Vector3(startX, playerObject.transform.position.y, startY-0.5f);
        // goalObject.transform.position = new Vector3(500, goalObject.transform.position.y, endY-0.5f);
    } 
}
