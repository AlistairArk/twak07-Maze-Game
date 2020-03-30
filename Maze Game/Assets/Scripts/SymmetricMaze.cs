using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymmetricMaze : MonoBehaviour{
    public MazeGlobals MazeGlobals;
    public Check Check;

    private int x;
    private int z;

    private List<List<int>> stack = new List<List<int>>();
    private List<List<int>> visited = new List<List<int>>();
    
    void Awake(){
        MazeGlobals = gameObject.GetComponent<MazeGlobals>();
        Check = gameObject.GetComponent<Check>();
    }



    public void Generate(int startX, int startZ){
        // print("Recursive Backtrack");

        int gridX = MazeGlobals.gridX;
        int gridZ = MazeGlobals.gridZ;

        // Set start points to last end point
        startX = MazeGlobals.endX;
        startZ = MazeGlobals.endZ;
        x = MazeGlobals.endX-1;
        z = MazeGlobals.endZ-1;

        // Reset distance counters
        MazeGlobals.endDist = -1;
        MazeGlobals.startDistance = 0;

        stack.Clear();
        visited.Clear();

        List<int> currentCell = new List<int>();
        currentCell.Add(x);
        currentCell.Add(z);

        stack.Add(currentCell);     // Place starting cell into stack
        visited.Add(currentCell);   // Add starting cell to visited list

        List<List<List<int>>> cellData = MazeGlobals.GetCellData();


        while(stack.Count > 0){
            /*
            Log cells relative distance from start
                - Used in deciding the optimal location of the goal cell
                - Path finding between start and end points
            */
            cellData[x][z][4] = MazeGlobals.startDistance;

            List<string> availableCells = new List<string>();

            // Check if adjacent cells are available (i.e. Unvisited)
            if (Check.CellIsAvailable(x,z+1, gridX, gridZ, visited)) availableCells.Add("n");
            if (Check.CellIsAvailable(x+1,z, gridX, gridZ, visited)) availableCells.Add("e");
            if (Check.CellIsAvailable(x,z-1, gridX, gridZ, visited)) availableCells.Add("s");
            if (Check.CellIsAvailable(x-1,z, gridX, gridZ, visited)) availableCells.Add("w");

            // Log cells relative distance from start
            cellData[x][z][4] = MazeGlobals.startDistance;
            


            if (availableCells.Count > 0){
                MazeGlobals.startDistance+=1; // Increment "Distance from start" counter

                // Randomly select an available adjacent cell
                int chosenCell = Random.Range(0, availableCells.Count);

                // Destroy wall between the current and adjacent cell
                // Move the x/z pointers to the next cell 
                if (availableCells[chosenCell] == "n"){

                    visited.Add(new List<int>{x,moveS(x,gridZ-1-z)});   // Top left
                    
                    // visited.Add(new List<int>{gridX-1-x,moveS(gridX-1-x,gridZ-1-z)});   // Top right

                    // visited.Add(new List<int>{gridX-1-x,moveS(gridX-1-x,z)});   // Bottom right
                    z = moveN(x,z);

                } else if (availableCells[chosenCell] == "e"){

                    visited.Add(new List<int>{moveE(x,gridZ-1-z), gridZ-1-z});   // Top left
                    
                    // visited.Add(new List<int>{moveW(gridX-1-x,gridZ-1-z), gridZ-1-z});   // Top right

                    // visited.Add(new List<int>{moveW(gridX-1-x,z), z});   // Top right
                    x = moveE(x,z);

                } else if (availableCells[chosenCell] == "s"){

                    visited.Add(new List<int>{x,moveN(x,gridZ-1-z)});   // Top left
                    
                    // visited.Add(new List<int>{gridX-1-x,moveN(gridX-1-x,gridZ-1-z)});   // Top right

                    // visited.Add(new List<int>{gridX-1-x,moveN(gridX-1-x,z)});   // Top right
                    z = moveS(x,z);

                } else if (availableCells[chosenCell] == "w"){

                    visited.Add(new List<int>{moveW(x,gridZ-1-z), gridZ-1-z});   // Top left
                    
                    // visited.Add(new List<int>{moveE(gridX-1-x,gridZ-1-z), gridZ-1-z});   // Top right

                    // visited.Add(new List<int>{moveE(gridX-1-x,z), z});   // Top right
                    x = moveW(x,z);
                }

                stack.Add(new List<int>{x,z});     // Add to visited list
                visited.Add(new List<int>{x,z});   // Place current cell on to stack

            }else{
                // Backtrack
                MazeGlobals.startDistance --;       // Decrement "distance from start" counter
                stack.RemoveAt(stack.Count - 1);    // Remove cell from stack
                if (stack.Count == 0) break;        // Break if stack is empty (i.e. "All cells visited")

                // Backtrack x/z cell pointers
                x = stack[stack.Count-1][0];
                z = stack[stack.Count-1][1];

            }
        }

        // Make center cut in order to connect sections of the maze

        // Place grid at ceneter of maze
        x = (int)((gridX-1)*.5f);
        z = (int)((gridZ-1)*.5f);
        z = moveN(x,z);
        x = moveE(x,z);
        z = moveS(x,z);
        x = moveW(x,z);
    }




    // Destroy walls and move pointer to next cell
    public int moveN(int x, int z){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][0] = 0;
        cellData[x][z+1][2] = 0;
                
        return (z+1); // Move pointer to the NORTH cell
    }

    // Destroy walls and move pointer to next cell
    public int moveE(int x, int z){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][1] = 0;
        cellData[x+1][z][3] = 0;
        
        return (x+1); // Move pointer to the EAST cell
    }

    // Destroy walls and move pointer to next cell
    public int moveS(int x, int z){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][2] = 0;
        cellData[x][z-1][0] = 0;
        
        return (z-1); // Move pointer to the SOUTH cell
    }

    // Destroy walls and move pointer to next cell
    public int moveW(int x, int z){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][3] = 0;
        cellData[x-1][z][1] = 0;
        
        return (x-1); // Move pointer to the WEST cell
    }
}