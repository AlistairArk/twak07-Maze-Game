using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePrefabs : MonoBehaviour {


    public MazeGlobals MazeGlobals;
    private int x;
    private int z;

    // Map Prefabs
    [Header("Tile Prefabs", order=1)]
    public GameObject CorridorHall;
    public GameObject CorridorEnd;
    public GameObject CorridorCorner;
    public GameObject CorridorTJunction;
    public GameObject CorridorIntersection;
    public GameObject Doorway;
    public GameObject Room2x3;
    public GameObject Room2x2;
    public GameObject RoomWall;
    public float prefabOffsetX = 0.5f;
    public float prefabOffsetZ = 0.5f;


    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
    }

    public void Corridors(){
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;
        int mapScale = MazeGlobals.mapScale;
        // int prefabOffsetX = MazeGlobals.prefabOffsetX;
        // int prefabOffsetZ = MazeGlobals.prefabOffsetZ;

        // List all candidates for start and end points

        // Reset the prefab list
        MazeGlobals.prefabList = new List<List<GameObject>>();

        for(int X = 0; X < gridX; X++){
            
            List<GameObject> prefabRow = new List<GameObject>();

            for(int Z = 0; Z < gridZ; Z++){
                
                int findCount = 0;

                for (int i = 0; i < 4; i++){
                    if (MazeGlobals.cellData[X][Z][i]==0) findCount++;
                }

                GameObject prefabCell;
                if (MazeGlobals.showPrefabMaze){
                    switch(findCount) {
                         case (1):
                            if (MazeGlobals.cellData[X][Z][0] == 0 ){
                                prefabCell = Instantiate(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                            } else if (MazeGlobals.cellData[X][Z][1] == 0){
                                prefabCell = Instantiate(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                            } else if (MazeGlobals.cellData[X][Z][2] == 0){
                                prefabCell = Instantiate(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            } else {
                                prefabCell = Instantiate(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                            }
                            prefabRow.Add(prefabCell);
                            break;
                        case (2):
                            // Hallways
                            if (MazeGlobals.cellData[X][Z][0] == 0  && MazeGlobals.cellData[X][Z][2] == 0 ){
                                prefabCell = Instantiate(CorridorHall, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            } else if (MazeGlobals.cellData[X][Z][1] == 0  && MazeGlobals.cellData[X][Z][3] == 0 ){
                                prefabCell = Instantiate(CorridorHall, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));

                            // Corners
                            } else if (MazeGlobals.cellData[X][Z][0] == 0  && MazeGlobals.cellData[X][Z][1] == 0 ){
                                prefabCell = Instantiate(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                            } else if (MazeGlobals.cellData[X][Z][1] == 0  && MazeGlobals.cellData[X][Z][2] == 0 ){
                                prefabCell = Instantiate(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            } else if (MazeGlobals.cellData[X][Z][2] == 0  && MazeGlobals.cellData[X][Z][3] == 0 ){
                                prefabCell = Instantiate(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                            } else{
                                prefabCell = Instantiate(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                            }
                            prefabRow.Add(prefabCell);
                            break;
                        case (3):
                            // T Junction
                            if (MazeGlobals.cellData[X][Z][0] == 1){
                                prefabCell = Instantiate(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                            } else if (MazeGlobals.cellData[X][Z][1] == 1){
                                prefabCell = Instantiate(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,180,0));
                            } else if (MazeGlobals.cellData[X][Z][2] == 1){
                                prefabCell = Instantiate(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,270,0));
                            } else{
                                prefabCell = Instantiate(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            }
                            prefabRow.Add(prefabCell);
                            break;
                        case (4):
                            prefabCell = Instantiate(CorridorIntersection, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,0,0));
                            prefabRow.Add(prefabCell);
                            break;
                    
                    }
                    prefabRow[prefabRow.Count-1].tag = "Occludable"; // Tag as prefab
                    prefabRow[prefabRow.Count-1].AddComponent<ObjectHider>();
                    prefabRow[prefabRow.Count-1].transform.parent = MazeGlobals.prefabMazeParent.transform; // Group objects
                    prefabRow[prefabRow.Count-1].name = X+","+Z; // Rename with co-ordinates
                }
            }
            MazeGlobals.prefabList.Add(prefabRow);
        }
        // SetSpawnPoints();
    }



  
    public void Rooms(){
        int mapScale = MazeGlobals.mapScale;
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;

        List<List<int>> roomCells1 = new List<List<int>>();
        List<List<List<int>>> rooms = new List<List<List<int>>>();
        roomCells1.Add(new List<int>{0,0});
        roomCells1.Add(new List<int>{0,1});
        roomCells1.Add(new List<int>{0,2});
        roomCells1.Add(new List<int>{1,0});
        roomCells1.Add(new List<int>{1,1});
        roomCells1.Add(new List<int>{1,2});
        rooms.Add(roomCells1);

        List<List<int>> roomCells2 = new List<List<int>>();
        roomCells2.Add(new List<int>{0,0});
        roomCells2.Add(new List<int>{0,1});
        roomCells2.Add(new List<int>{1,1});
        roomCells2.Add(new List<int>{1,0});
        rooms.Add(roomCells2);

        int counter = 0;

        // Iterate over elements in optimal path
        foreach(List<int> pathElement in MazeGlobals.optimalPath){
            int prefabCounter = 0;
            counter++;

            int X = pathElement[0];
            int Z = pathElement[1];
            foreach(List<List<int>> room in rooms){
                prefabCounter++;

                int cellCount = room.Count;
                int cellX = 0;
                int cellZ = 0;

                foreach(List<int> cell in room){
                    cellX = X+cell[0];
                    cellZ = Z+cell[1];


                    // If cell is in grid && cell is on a path && cell is not room
                    if (cellX<gridX && cellZ<gridZ && MazeGlobals.cellData[cellX][cellZ][5]==1 && MazeGlobals.cellData[cellX][cellZ][6]==0){
                        cellCount--;
                    }else{
                        break;
                    }
                }

                
                if (cellCount==0){
                    // Set global X & Z
                    x=X;
                    z=Z;
                    GameObject prefabRoom;
                    // Place the room prefab
                    switch(prefabCounter) {
                        case (1):
                            // prefabRoom = Instantiate(Room2x3, new Vector3((X+1f)*mapScale, 0f, (Z+1.5f)*mapScale), Quaternion.Euler(0,90,0));
                            // prefabRoom.transform.parent = MazeGlobals.cellDoorParent.transform;
                        break;

                        case (2):
                            // prefabRoom = Instantiate(Room2x2, new Vector3((X+1f)*mapScale, 0f, (Z+1f)*mapScale), Quaternion.Euler(0,90,0));
                            // prefabRoom.transform.parent = MazeGlobals.cellDoorParent.transform;
                        break;
                    }
                    
                    // Remove walls in room cells
                    foreach(List<int> cell in room){
                        print("placing room");
                        cellX = cell[0]+X;
                        cellZ = cell[1]+Z;

                        roomCell(cellX,cellZ,room); // n
                    }
                }
            }
        }
    }



    public void roomCell(int X, int Z, List<List<int>> room){

        // Disable walls in same room
        foreach(List<int> cell in room){

            int cellX = x+cell[0];
            int cellZ = z+cell[1];

            if (cellX == X && cellZ == Z+1){
                MazeGlobals.cellList[X][Z][0].SetActive(false);
            }else if (cellX == X+1 && cellZ == Z){
                MazeGlobals.cellList[X][Z][1].SetActive(false);
            }else if (cellX == X && cellZ == Z-1){
                MazeGlobals.cellList[X][Z-1][0].SetActive(false);
            }else if (cellX == X-1 && cellZ == Z){
                MazeGlobals.cellList[X-1][Z][1].SetActive(false);
            }
        }

        MazeGlobals.cellData[X][Z][6]=1;        // Mark cell as room

        // print("(X, Z) ("+X+", "+Z+")");
        // Destroy(MazeGlobals.prefabList[X][Z]);
    }



    public void placeDoors(int X, int Z){
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;
        float mapScale = MazeGlobals.mapScale;

        // Run code to place the door here, otherwise you may run the issue of rooms being connected when they shouldn't

        GameObject cellDoor;
        // If, in grid && next cell is not room
        if (X<gridX && Z+1<gridZ && MazeGlobals.cellData[X][Z+1][6]==0){
            switch(MazeGlobals.cellData[X][Z][0]){
                case(0): // If Hallway, make door
                cellDoor = Instantiate(Doorway, new Vector3((X+prefabOffsetX)*mapScale, 0, (Z+1f)*mapScale), Quaternion.Euler(0,0,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
                case(1): // If wall, make wall
                cellDoor = Instantiate(RoomWall, new Vector3(X*mapScale, 0f, (Z+.95f)*mapScale), Quaternion.Euler(0,180,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
            }
        }

        if (X+1<gridX && Z<gridZ && MazeGlobals.cellData[X+1][Z][6]==0){
            switch(MazeGlobals.cellData[X][Z][1]){
                case(0): // If Hallway, make door
                cellDoor = Instantiate(Doorway, new Vector3((X+1f)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
                case(1): // If wall, make wall
                cellDoor = Instantiate(RoomWall, new Vector3((X+1f)*mapScale, 0f, (Z+.95f)*mapScale), Quaternion.Euler(0,270,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                // cellDoor = Instantiate(RoomWall, new Vector3((X+.95f)*mapScale, 0f, (Z+1f*mapScale)), Quaternion.Euler(0,270,0));
                // cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
            }
        }

        if (X<gridX && Z-1>=0 && MazeGlobals.cellData[X][Z-1][6]==0){
            switch(MazeGlobals.cellData[X][Z][2]){
                case(0): // If Hallway, make door
                cellDoor = Instantiate(Doorway, new Vector3((X+prefabOffsetX)*mapScale, 0, Z*mapScale), Quaternion.Euler(0,0,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
                case(1): // If wall, make wall
                cellDoor = Instantiate(RoomWall, new Vector3((X+1f)*mapScale, 0f, (Z+0.05f)*mapScale), Quaternion.Euler(0,0,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                // cellDoor = Instantiate(RoomWall, new Vector3((X+.5f)*mapScale, 0f, (Z+0.05f)*mapScale), Quaternion.Euler(0,0,0));
                // cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
            }
        }

        if (X-1>=0 && Z<gridZ && MazeGlobals.cellData[X-1][Z][6]==0){
            switch(MazeGlobals.cellData[X][Z][3]){
                case(0): // If Hallway, make door
                cellDoor = Instantiate(Doorway, new Vector3((X)*mapScale, 0, (Z+prefabOffsetZ)*mapScale), Quaternion.Euler(0,90,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;

                case(1): // If wall, make wall
                cellDoor = Instantiate(RoomWall, new Vector3((X+0.05f)*mapScale, 0f, Z*mapScale), Quaternion.Euler(0,90,0));
                cellDoor.transform.parent = MazeGlobals.cellDoorParent.transform;
                break;
            }
        }
    }
}
