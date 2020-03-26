using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour {


    public MazeGlobals MazeGlobals;

    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
    }

    public bool CellIsAvailable(int cellX, int cellZ, int gridX, int gridZ, List<List<int>> visited){
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


    public void CellEnds(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;

        int endDist = -1;
        int endX = 0;
        int endZ = 0;

        // List all candidates for start and end points
        for(int X = 0; X < gridX; X++){
            for(int Z = 0; Z < gridZ; Z++){
                
                int findCount = 0;
                for (int i = 0; i < 4; i++) if (cellData[X][Z][i]==0) findCount++;

                // If end cell is further than the current end cell
                if (findCount==1 && (cellData[X][Z][4]>endDist  ||  endDist==-1)){
                    
                    // Set new end point
                    endX = X+1;
                    endZ = Z+1;
                    endDist = cellData[X][Z][4];
                }
            }
        }

        MazeGlobals.endX = endX;    // Ideal Goal position
        MazeGlobals.endZ = endZ;    // Ideal Goal position
        MazeGlobals.endDist = endDist;
    }



    public void MainPath(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        MazeGlobals.optimalPath.Clear();
        bool endFound = false;
        int counter = 0;
        int x = MazeGlobals.endX-1;
        int z = MazeGlobals.endZ-1;

        int distance;

        // Get starting distance based off goal location
        // print("Start Point ("+x+", "+z+")");

        distance = cellData[x][z][4];
        while (!endFound){
            counter++;

            // Find neighboring cell with lowest distance from start

            // If there is a adjacent path && distance is less than previous (i.e. getting closer to the start.)
            if (cellData[x][z][0]==0 && cellData[x][z+1][4]<distance){ // North
                distance=cellData[x][z+1][4]; z++;

            }else if (cellData[x][z][1]==0 && cellData[x+1][z][4]<distance){ // East
                distance=cellData[x+1][z][4]; x++;

            }else if (cellData[x][z][2]==0 && cellData[x][z-1][4]<distance){ // South
                distance=cellData[x][z-1][4]; z--;

            }else if (cellData[x][z][3]==0 && cellData[x-1][z][4]<distance){ // West
                distance=cellData[x-1][z][4]; x--;
            }

            // if (MazeGlobals.hideWaypoint==false) GuideCube(x,z);

            cellData[x][z][5]=1;   // Mark cell as on path
            MazeGlobals.optimalPath.Add(new List<int>{x,z});


            if (distance==0){
                endFound = true;
            }
        }

    }
}
