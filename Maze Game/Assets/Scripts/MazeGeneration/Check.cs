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

        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;

        int endDist = -1;
        int endX = 0;
        int endZ = 0;

        // List all candidates for start and end points
        for(int X = 0; X < gridX; X++){
            for(int Z = 0; Z < gridZ; Z++){
                
                int findCount = 0;
                for (int i = 0; i < 4; i++) if (MazeGlobals.cellData[X][Z][i]==0) findCount++;

                // If end cell is further than the current end cell
                if (findCount==1 && (MazeGlobals.cellData[X][Z][4]>endDist  ||  endDist==-1)){
                    
                    // Set new end point
                    endX = X+1;
                    endZ = Z+1;
                    endDist = MazeGlobals.cellData[X][Z][4];
                }
            }
        }

        MazeGlobals.endX = endX;
        MazeGlobals.endZ = endZ;
        MazeGlobals.endDist = endDist;
    }



    public void MainPath(){

        MazeGlobals.optimalPath.Clear();
        bool endFound = false;
        int counter = 0;
        int x = MazeGlobals.endX-1;
        int z = MazeGlobals.endZ-1;

        int distance;

        // Get starting distance based off goal location
        // print("Start Point ("+x+", "+z+")");

        distance = MazeGlobals.cellData[x][z][4];
        while (!endFound){
            counter++;

            // Find neighboring cell with lowest distance from start

            // If there is a adjacent path && distance is less than previous (i.e. getting closer to the start.)
            if (MazeGlobals.cellData[x][z][0]==0 && MazeGlobals.cellData[x][z+1][4]<distance){ // North
                distance=MazeGlobals.cellData[x][z+1][4]; z++;

            }else if (MazeGlobals.cellData[x][z][1]==0 && MazeGlobals.cellData[x+1][z][4]<distance){ // East
                distance=MazeGlobals.cellData[x+1][z][4]; x++;

            }else if (MazeGlobals.cellData[x][z][2]==0 && MazeGlobals.cellData[x][z-1][4]<distance){ // South
                distance=MazeGlobals.cellData[x][z-1][4]; z--;

            }else if (MazeGlobals.cellData[x][z][3]==0 && MazeGlobals.cellData[x-1][z][4]<distance){ // West
                distance=MazeGlobals.cellData[x-1][z][4]; x--;
            }

            // if (MazeGlobals.hideWaypoint==false) GuideCube(x,z);

            MazeGlobals.cellData[x][z][5]=1;   // Mark cell as on path
            MazeGlobals.optimalPath.Add(new List<int>{x,z});


            if (distance==0){
                endFound = true;
            }
        }
    }
}
