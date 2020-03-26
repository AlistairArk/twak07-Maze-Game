using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour{


    public GameObject GoalCube;
    public GameObject GoalGroup;
    private GoalDetector GoalDetector;

    public RecursiveBacktrack RecursiveBacktrack;
    public InitializeMaze InitializeMaze;
    public SymmetricMaze SymmetricMaze;
    public MazePrefabs MazePrefabs;
    public MazeGlobals MazeGlobals;
    public ResetMaze ResetMaze;
    public Check Check;

    private int startX = 0;
    private int startZ = 0;

    private GameObject stationCam;
    private Camera cam1;
    private GameObject hackerCam;
    private Camera cam2;

    // Called before any Start function
    void Awake(){
        GoalDetector = GoalCube.GetComponent<GoalDetector>();

        RecursiveBacktrack  = gameObject.GetComponent<RecursiveBacktrack>();
        InitializeMaze      = gameObject.GetComponent<InitializeMaze>();
        SymmetricMaze       = gameObject.GetComponent<SymmetricMaze>();
        MazePrefabs         = gameObject.GetComponent<MazePrefabs>();
        MazeGlobals         = gameObject.GetComponent<MazeGlobals>();
        ResetMaze           = gameObject.GetComponent<ResetMaze>();
        Check               = gameObject.GetComponent<Check>();
    }

    void Start(){
        InitializeMaze.Initialize();
        GenerateSpaceStation();

        cam1 = GameObject.FindWithTag("StationCam").GetComponent<Camera>();
        cam2 = GameObject.FindWithTag("HackingCam").GetComponent<Camera>();
        cam2.enabled = false;
    }


    void GenerateSpaceStation(){
        // RecursiveBacktrack.Generate(startX,startZ);
        SymmetricMaze.Generate(startX,startZ);

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

        SetGoalPosition();
    }

    public void SetGoalPosition(){
        float gX, gZ = 0f;
        gX  = (MazeGlobals.endX-.5f) * MazeGlobals.mapScale;
        gZ  = (MazeGlobals.endZ-.5f) * MazeGlobals.mapScale;

        GoalGroup.transform.position = new Vector3(gX, GoalGroup.transform.position.y, gZ);
    }



    // Update is called once per frame
    void Update(){
        // Debug.Log(GoalDetector.goalTrigger);
        if (GoalDetector.goalTrigger){
            GoalDetector.goalTrigger = false;

            MazeGlobals.mode = 0;
            ResetMaze.Reset();
            GenerateSpaceStation();
        }
    }


    public void HackingGame(){
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        
        MazeGlobals.mode = 1;

        ResetMaze.Reset();
        SymmetricMaze.Generate(startX,startZ);

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

        SetGoalPosition();
    }
}