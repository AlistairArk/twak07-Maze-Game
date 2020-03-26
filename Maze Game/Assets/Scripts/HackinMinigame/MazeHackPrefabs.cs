using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHackPrefabs : MonoBehaviour{

    public MazeGlobals MazeGlobals;
    private int x;
    private int z;

    // Map Prefabs
    [Header("Corridors", order=1)]
    public GameObject CorridorHall;
    // public GameObject CorridorHallExterior;
    public GameObject CorridorEnd;
    public GameObject CorridorCorner;
    public GameObject CorridorTJunction;
    public GameObject CorridorIntersection;

    [Header("Misc.", order=5)]
    public float prefabOffsetX = 0.5f;
    public float prefabOffsetZ = 0.5f;


    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
    }

    public void Corridors(){
        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;
        int mapScale = MazeGlobals.mapScale;

        float mapHeight = 0f;
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

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
                    if (cellData[X][Z][i]==0) findCount++;
                }

                if (MazeGlobals.showPrefabMaze){
                    switch(findCount) {
                         case (1):
                            if (cellData[X][Z][0] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,180,0)));
                            } else if (cellData[X][Z][1] == 0){
                                prefabRow.Add(NewPrefab(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,270,0)));
                            } else if (cellData[X][Z][2] == 0){
                                prefabRow.Add(NewPrefab(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,0,0)));
                            } else {
                                prefabRow.Add(NewPrefab(CorridorEnd, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,90,0)));
                            }
                            break;
                        case (2):
                            // Hallways
                            if (cellData[X][Z][0] == 0  && cellData[X][Z][2] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorHall, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,0,0)));

                            } else if (cellData[X][Z][1] == 0  && cellData[X][Z][3] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorHall, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,90,0)));

                            // Corners
                            } else if (cellData[X][Z][0] == 0  && cellData[X][Z][1] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,270,0)));
                            } else if (cellData[X][Z][1] == 0  && cellData[X][Z][2] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,0,0)));
                            } else if (cellData[X][Z][2] == 0  && cellData[X][Z][3] == 0 ){
                                prefabRow.Add(NewPrefab(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,90,0)));
                            } else{
                                prefabRow.Add(NewPrefab(CorridorCorner, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,180,0)));
                            }
                            break;
                        case (3):
                            // T Junction
                            if (cellData[X][Z][0] == 1){
                                prefabRow.Add(NewPrefab(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,90,0)));
                            } else if (cellData[X][Z][1] == 1){
                                prefabRow.Add(NewPrefab(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,180,0)));
                            } else if (cellData[X][Z][2] == 1){
                                prefabRow.Add(NewPrefab(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,270,0)));
                            } else{
                                prefabRow.Add(NewPrefab(CorridorTJunction, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,0,0)));
                            }
                            break;
                        case (4):
                            prefabRow.Add(NewPrefab(CorridorIntersection, new Vector3((X+prefabOffsetX)*mapScale, mapHeight, (Z+prefabOffsetZ)*mapScale), new Vector3(0,0,0)));
                            break;
                    
                    }
                    // prefabRow[prefabRow.Count-1].tag = "Occludable"; // Tag as prefab
                    // prefabRow[prefabRow.Count-1].AddComponent<ObjectHider>();
                    // prefabRow[prefabRow.Count-1].transform.parent = MazeGlobals.prefabHackParent.transform; // Group objects
                    prefabRow[prefabRow.Count-1].name = X+","+Z; // Rename with co-ordinates
                }
            }
            MazeGlobals.prefabList.Add(prefabRow);
        }
        // SetSpawnPoints();
    }

    public GameObject NewPrefab(GameObject prefab, Vector3 pos, Vector3 rot){

        GameObject prefabCell;
        prefabCell = Instantiate(prefab, pos, Quaternion.Euler(rot.x,rot.y,rot.z));
        prefabCell.transform.parent = MazeGlobals.prefabHackParent.transform;
        prefabCell.transform.localPosition = pos;
        prefabCell.layer = 11; // Set layer to "HackingCam" to make it viewable by the camera
        return prefabCell;
    }
}
