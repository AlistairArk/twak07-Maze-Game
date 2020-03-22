using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour{


    public GameObject GoalObj;
    private GoalDetector GoalDetector;
    private WallPlacer WallPlacer;


    // Start is called before the first frame update
    void Start(){
        GoalDetector = GoalObj.GetComponent<GoalDetector>();

        WallPlacer = gameObject.GetComponent<WallPlacer>();
        WallPlacer.Start();
        WallPlacer.startMap();
        WallPlacer.HidePrefabs();

    }

    // Update is called once per frame
    void Update(){
        // Debug.Log(GoalDetector.goalTrigger);
        if (GoalDetector.goalTrigger){
            WallPlacer.resetMap();
            GoalDetector.goalTrigger = false;
        }
    }
}
