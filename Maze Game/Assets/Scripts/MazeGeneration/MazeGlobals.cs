/*

Stores all the golbal variables used across classes in maze generation.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGlobals : MonoBehaviour{

    [Header("Maze Settings", order=0)]
    public int gridX;
    public int gridZ;
    public int endX;
    public int endZ;

    public bool showRawMaze = true;
    public bool showPrefabMaze = true;
    public bool hideBase = true;
    public bool hideWalls = true;
    public bool hideWaypoint = true;




    // RawMaze
    [Header("Raw Maze", order=2)]
    public GameObject wall;
    public GameObject[ ] walls;
    public GameObject goalObject;
    public GameObject playerObject;

    public List<List<List<GameObject>>> cellList = new List<List<List<GameObject>>>();
    public List<List<List<int>>> cellData = new List<List<List<int>>>();

    [Header("Materials", order=3)]
    public Material wallMat;
    public Material playerMat;
    public Material whiteMat;

    [Header("Misc.", order=4)]

    [HideInInspector] public float gridOffset;
    [HideInInspector] public int x;
    [HideInInspector] public int z;
    [HideInInspector] public int endDist = -1;
    [HideInInspector] public int mapScale = 10;
    [HideInInspector] public int startDistance = 0;
    [HideInInspector] public float wallHeight = .3f;
    
    [HideInInspector] public List<List<int>> optimalPath = new List<List<int>>();
    
    [HideInInspector] public List<List<GameObject>> prefabList = new List<List<GameObject>>();
    [HideInInspector] public CharacterController charController;
    [HideInInspector] public GameObject prefabMazeParent;
    [HideInInspector] public GameObject rawMazeParent;
    [HideInInspector] public GameObject guideCubeParent;
    [HideInInspector] public GameObject cellDoorParent;
    [HideInInspector] public GameObject cellWallParent;
    [HideInInspector] public GameObject mapObjects;


    public void Awake(){
        charController = playerObject.GetComponent<CharacterController>();

        rawMazeParent = GameObject.Find("MapObjects/rawMazeParent");
        guideCubeParent = GameObject.Find("MapObjects/guideCubeParent");
        prefabMazeParent = GameObject.Find("MapObjects/prefabMazeParent");
        cellDoorParent = GameObject.Find("MapObjects/cellDoorParent");
        cellWallParent = GameObject.Find("MapObjects/cellWallParent");
        mapObjects = GameObject.Find("MapObjects");

    }
}
