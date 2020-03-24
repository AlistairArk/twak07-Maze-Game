using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour{


    public GameObject GoalObj;
    private GoalDetector GoalDetector;

    public RecursiveBacktrack RecursiveBacktrack;
    public InitializeMaze InitializeMaze;
    public MazePrefabs MazePrefabs;
    public MazeGlobals MazeGlobals;
    public ResetMaze ResetMaze;
    public Check Check;

    private int startX = 0;
    private int startZ = 0;

    // Called before any Start function
    void Awake(){
        GoalDetector = GoalObj.GetComponent<GoalDetector>();

        RecursiveBacktrack  = gameObject.GetComponent<RecursiveBacktrack>();
        InitializeMaze      = gameObject.GetComponent<InitializeMaze>();
        MazePrefabs         = gameObject.GetComponent<MazePrefabs>();
        MazeGlobals         = gameObject.GetComponent<MazeGlobals>();
        ResetMaze           = gameObject.GetComponent<ResetMaze>();
        Check               = gameObject.GetComponent<Check>();
    }

    void Start(){
        InitializeMaze.Initialize();
        RecursiveBacktrack.Generate(startX,startZ);

        Check.CellEnds();

        MazePrefabs.Corridors();
        
        Check.MainPath(); // Crawl through the maze and find the main path

        MazePrefabs.Rooms(); // Interspace rooms along that path

        float pcPosX = (startX+.5f) * MazeGlobals.mapScale;
        float pcPosZ = (startZ+.5f) * MazeGlobals.mapScale;

        // Temporarily disable controller while repositioning the player object
        MazeGlobals.charController.enabled = false;
        MazeGlobals.playerObject.transform.position = new Vector3(pcPosX, MazeGlobals.playerObject.transform.position.y, pcPosZ);
        MazeGlobals.charController.enabled = true;
    }

    // Update is called once per frame
    void Update(){
        // Debug.Log(GoalDetector.goalTrigger);
        if (GoalDetector.goalTrigger){
            GoalDetector.goalTrigger = false;
        }
    }
}



        // InitializeMaze = gameObject.GetComponent<InitializeMaze>();
        // ResetMaze = gameObject.GetComponent<ResetMaze>();

            // WallPlacer.resetMap();
        
        // WallPlacer = gameObject.GetComponent<WallPlacer>();
        // WallPlacer.Start();
        // WallPlacer.startMap();
        // WallPlacer.HidePrefabs();