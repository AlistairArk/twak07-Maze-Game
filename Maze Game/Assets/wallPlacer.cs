using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallPlacer : MonoBehaviour{

    public GameObject wall;
    public GameObject[ ] walls;
    public Material wallMat;
    public GameObject goalObject;
    public GameObject playerObject;

    public List<List<List<GameObject>>> cellList = new List<List<List<GameObject>>>();
    public List<List<List<int>>> cellWalls = new List<List<List<int>>>();


    public List<List<int>> stack = new List<List<int>>();
    public List<List<int>> visited = new List<List<int>>();

    public int gridX;
    public int gridZ;

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

    int startDistance = 0;

    // Start is called before the first frame update
    public void startMap() {

        for(int cellX = 0; cellX < gridX; cellX++){
            List<List<GameObject>> cellRow = new List<List<GameObject>>();
            List<List<int>> cellWallsRow = new List<List<int>>();
            for(int cellY = 0; cellY < gridZ; cellY++){

                List<GameObject> cell = new List<GameObject>();
                List<int> cellWallsGroup = new List<int>();

                GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube1.transform.localScale = new Vector3(.25f, 1, 1);
                cube1.transform.Rotate(0f, 90f, 0f);
                cube1.GetComponent<Renderer>().material = wallMat;
                cell.Add(cube1); // North Wall

                GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.transform.localScale = new Vector3(.25f, 1, 1);
                cube2.GetComponent<Renderer>().material = wallMat;
                cell.Add(cube2); // East Wall

                GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube3.transform.localScale = new Vector3(.25f, 1, 1);
                cube3.transform.Rotate(0f, 90f, 0f);
                cube3.GetComponent<Renderer>().material = wallMat;
                cell.Add(cube3); // South Wall

                GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube4.transform.localScale = new Vector3(.25f, 1, 1);
                cube4.GetComponent<Renderer>().material = wallMat;
                cell.Add(cube4); // West Wall

                cube1.transform.position = new Vector3(cellX, wallHeight, cellY);
                cube2.transform.position = new Vector3(cellX+.5f, wallHeight, cellY-.5f);
                cube3.transform.position = new Vector3(cellX, wallHeight, cellY-1f);
                cube4.transform.position = new Vector3(cellX-.5f, wallHeight, cellY-.5f);
                

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


        // Build Grid
        for(int cellX = 0; cellX < gridX-1; cellX++){
            for(int cellY = 0; cellY < gridZ-1; cellY++){
                placeCell(cellX,cellY);
            }
        }

        // Create level base
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3((gridX*.5f)-.5f, 0, (gridZ*.5f)-1);
        cube.transform.localScale = new Vector3(gridX, 0.01f, gridZ);
        cube.layer = 8;

        recursiveBacktrack(gridX-1,gridZ-1); // Build Maze
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
        // Destroy(cellList[x][y][0]);
        // Destroy(cellList[x][y+1][2]);
        cellList[x][y][0].SetActive(false);
        cellList[x][y+1][2].SetActive(false);
    }

    public void moveE(){

        cellWalls[x][y][1] = 0;
        cellWalls[x+1][y][3] = 0;
        // Destroy(cellList[x][y][1]);
        // Destroy(cellList[x+1][y][3]);
        cellList[x][y][1].SetActive(false);
        cellList[x+1][y][3].SetActive(false);
    }

    public void moveS(){

        cellWalls[x][y][2] = 0;
        cellWalls[x][y-1][0] = 0;
        // Destroy(cellList[x][y][2]);
        // Destroy(cellList[x][y-1][0]);
        cellList[x][y][2].SetActive(false);
        cellList[x][y-1][0].SetActive(false);
    }

    public void moveW(){

        cellWalls[x][y][3] = 0;
        cellWalls[x-1][y][1] = 0;

        // Destroy(cellList[x][y][3]);
        // Destroy(cellList[x-1][y][1]);
        cellList[x][y][3].SetActive(false);
        cellList[x-1][y][1].SetActive(false);
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
        print("Wowee");
    }


    // Update is called once per frame
    public void placeCell(int cellX, int cellY){
        cellList[cellX][cellY][0].transform.position = new Vector3(cellX, wallHeight, cellY);
        cellList[cellX][cellY][1].transform.position = new Vector3(cellX+.5f, wallHeight, cellY-.5f);
        cellList[cellX][cellY][2].transform.position = new Vector3(cellX, wallHeight, cellY-1f);
        cellList[cellX][cellY][3].transform.position = new Vector3(cellX-.5f, wallHeight, cellY-.5f);
    }

    public void getCellEnds(){
        // List all candidates for start and end points


        for(int cellX = 0; cellX < gridX-1; cellX++){
            for(int cellY = 0; cellY < gridZ-1; cellY++){
                
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
            }
        }
        setSpawnPoints();
    }


    public void setSpawnPoints(){
        goalObject.transform.position = new Vector3(endX, goalObject.transform.position.y, endY-0.5f);
        playerObject.transform.position = new Vector3(startX, playerObject.transform.position.y, startY-0.5f);
    }

    public void resetMap(){

        for(int cellX = 0; cellX < gridX; cellX++){
            for(int cellY = 0; cellY < gridZ; cellY++){


                cellWalls[cellX][cellY][0] = 1;
                cellWalls[cellX][cellY][1] = 1;
                cellWalls[cellX][cellY][2] = 1;
                cellWalls[cellX][cellY][3] = 1;
                cellWalls[cellX][cellY][4] = 0;

                cellList[cellX][cellY][0].SetActive(true);
                cellList[cellX][cellY][1].SetActive(true);
                cellList[cellX][cellY][2].SetActive(true);
                cellList[cellX][cellY][3].SetActive(true);
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
        print("HELLO");

        recursiveBacktrack(gridX-1,gridZ-1); // Build Maze
        goalObject.transform.position = new Vector3(endX, goalObject.transform.position.y, endY-0.5f);
        playerObject.transform.position = new Vector3(startX, playerObject.transform.position.y, startY-0.5f);
        // goalObject.transform.position = new Vector3(500, goalObject.transform.position.y, endY-0.5f);
    } 
}
