using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktrack : MonoBehaviour {

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
                    moveN();
                } else if (availableCells[chosenCell] == "e"){
                    moveE();
                } else if (availableCells[chosenCell] == "s"){
                    moveS();
                } else if (availableCells[chosenCell] == "w"){
                    moveW();
                }

                List<int> nextCell = new List<int>();
                nextCell.Add(x);
                nextCell.Add(z);
                stack.Add(nextCell);     // Add to visited list
                visited.Add(nextCell);   // Place current cell on to stack

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
    }


    // Destroy walls and move pointer to next cell
    public void moveN(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][0] = 0;
        cellData[x][z+1][2] = 0;
                
        z+=1; // Move pointer to the NORTH cell
    }

    // Destroy walls and move pointer to next cell
    public void moveE(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][1] = 0;
        cellData[x+1][z][3] = 0;
        
        x+=1; // Move pointer to the EAST cell
    }

    // Destroy walls and move pointer to next cell
    public void moveS(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][2] = 0;
        cellData[x][z-1][0] = 0;
        
        z-=1; // Move pointer to the SOUTH cell
    }

    // Destroy walls and move pointer to next cell
    public void moveW(){
        List<List<List<int>>> cellData = MazeGlobals.GetCellData();

        cellData[x][z][3] = 0;
        cellData[x-1][z][1] = 0;
        
        x-=1; // Move pointer to the WEST cell
    }
}
