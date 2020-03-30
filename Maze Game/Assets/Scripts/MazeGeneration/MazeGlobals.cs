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
    public int mode = 0;
    public int type = 0; // 0 Recursive .. 1 Symmetric

    // RawMaze
    [Header("Raw Maze", order=2)]
    public GameObject wall;
    public GameObject[] walls;
    public GameObject goalObject;
    public GameObject playerObject;
    public GameObject playerGroup;

    public List<List<List<GameObject>>> cellList = new List<List<List<GameObject>>>();
    public List<List<List<GameObject>>> cellListHack = new List<List<List<GameObject>>>();
    public List<List<List<int>>> cellData = new List<List<List<int>>>();
    public List<List<List<int>>> cellDataHack = new List<List<List<int>>>();

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
    [HideInInspector] public GameObject prefabHackParent;
    [HideInInspector] public GameObject cellBaseParent;
    [HideInInspector] public GameObject mapObjects;


    public void Awake(){
        charController = playerObject.GetComponent<CharacterController>();

        rawMazeParent = GameObject.Find("SpaceStation/MazeObjects/rawMazeParent");
        guideCubeParent = GameObject.Find("SpaceStation/MazeObjects/guideCubeParent");
        prefabMazeParent = GameObject.Find("SpaceStation/MazeObjects/prefabMazeParent");
        cellDoorParent = GameObject.Find("SpaceStation/MazeObjects/cellDoorParent");
        cellWallParent = GameObject.Find("SpaceStation/MazeObjects/cellWallParent");
        cellBaseParent = GameObject.Find("SpaceStation/MazeObjects/cellBaseParent");
        mapObjects = GameObject.Find("SpaceStation/MazeObjects");

        prefabHackParent = GameObject.Find("HackingMinigame/Platform/MazeObjects");
    }


    public List<List<List<int>>> GetCellData(){
        if (mode==0) return (cellData);
        else if (mode==1) return (cellDataHack);
        else return (cellData);
    }

    public List<List<List<GameObject>>> GetCellList(){
        if (mode==0) return (cellList);
        else if (mode==1) return (cellListHack);
        else return (cellList);
    }
}
