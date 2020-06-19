using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour {

    public bool doorOpen = false;
    public bool doorLastState = false;  // The status of the door at last update
    public bool doorLocked = true;      // Will become unlocked after hacking

    /*
    doorStatus
        0 - Door is in a fixed position
        1 - Door is closing
        2 - Door is opening
    */
    public int doorStatus = 0;
    public int animSpeed = 1;   // Speed at which the door opens and closes.

    public GameObject terminalA;
    public GameObject terminalB;

    public GameObject doorA;
    public GameObject doorB;

    public Vector3 openStateA; // Open state vectors for door A
    public Vector3 openStateB; // Open state vectors for door B

    public Vector3 closedStateA; // Open state vectors for door A
    public Vector3 closedStateB; // Open state vectors for door B

    private PlayerManager PlayerManager;
    private MazeGenerator MazeGenerator;




    void Start(){
        closedStateA = doorA.transform.localPosition;
        closedStateB = doorB.transform.localPosition;

        MazeGenerator = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        PlayerManager = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>();

        // Check player manager door lock probability
        float randValue = Random.Range(0f, 100.0f)*0.001f;
        if (randValue > PlayerManager.doorLockChance){
            doorLocked = false;
        }else{
            doorLocked = true;
        }
    }



    // Update is called once per frame
    void Update(){

        // Doorway trigger management
        bool triggerA = terminalA.GetComponent<TerminalTrigger>().trigger;
        bool triggerB = terminalB.GetComponent<TerminalTrigger>().trigger;

        if (triggerA||triggerB){
            // Reset trigger
            terminalA.GetComponent<TerminalTrigger>().trigger = false;
            terminalB.GetComponent<TerminalTrigger>().trigger = false;

            if (doorStatus==0){
                // Trigge open/close
                doorOpen=!doorOpen;
            }
        }

        // Animation system
        if (doorStatus==1){
            OpenAnim();
        }else if(doorStatus==2){
            CloseAnim();
        }else{
            if (doorOpen&&!doorLastState){
                if (!doorLocked) doorStatus=1; // Door OPEN has been triggered
                else MazeGenerator.HackingGame(gameObject,4,4); // Hand reference for win/loss callback
            }else if (!doorOpen&&doorLastState){
                doorStatus=2; // Door CLOSE has been triggered
            }
        }
     
    

        doorLastState = doorOpen;
    }



    void CloseAnim(){
        // Move our position a step closer to the target.
        float step =  animSpeed * Time.deltaTime; // calculate distance to move
        doorA.transform.localPosition = Vector3.Lerp(doorA.transform.localPosition, closedStateA, step);
        doorB.transform.localPosition = Vector3.Lerp(doorB.transform.localPosition, closedStateB, step);

        // Check if the position of the door and targer position
        if (Vector3.Distance(doorA.transform.localPosition, closedStateA) < 0.01f 
            && Vector3.Distance(doorB.transform.localPosition, closedStateB) < 0.01f)
            doorStatus = 0;
    }



    void OpenAnim(){
        // Move our position a step closer to the target.
        float step =  animSpeed * Time.deltaTime; // calculate distance to move
        doorA.transform.localPosition = Vector3.Lerp(doorA.transform.localPosition, openStateA, step);
        doorB.transform.localPosition = Vector3.Lerp(doorB.transform.localPosition, openStateB, step);

        // Check if the position of the door and targer position
        if (Vector3.Distance(doorA.transform.localPosition, openStateA) < 0.01f 
            && Vector3.Distance(doorB.transform.localPosition, openStateB) < 0.01f)
            doorStatus = 0;
    }


    public void HackWin(){
        doorLocked=false;
    }

    public void HackLoss(){
        doorLocked=true;
    } 
}
