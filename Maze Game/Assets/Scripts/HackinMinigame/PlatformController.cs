using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour{

    public float speed = 10f;
    
    public GameObject ball; 
    public GameObject platform;
    public GameObject DeathZoneObject;
    public int deathZoneGap = 50;
    public int tries = 5;


    private DeathZone DeathZone;
    private MazeGenerator MazeGenerator;
    private GoalDetector GoalDetector;

    // Start is called before the first frame update
    void Awake(){
        DeathZone = DeathZoneObject.GetComponent<DeathZone>();
        MazeGenerator = GameObject.FindWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        GoalDetector = GameObject.FindWithTag("HackGoal").GetComponent<GoalDetector>();
    }



    public void ResetBall(){
        ball.transform.localPosition = new Vector3(5f, 10f, 5f);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        tries--;
        print("TRIES LEFT: "+tries);
        if (tries <= 0) MazeGenerator.HackingGameLoss(); // Just win for now
    }


    public void ResetTries(){ // Call on startup
        tries=5; 

        // Reset platform rotation
        platform.transform.rotation = Quaternion.identity;
    }


    void Update()    {

        Vector3 ballPos = ball.transform.position;
        Vector3 platformPos = platform.transform.position;
        Vector3 deathZonePos = DeathZoneObject.transform.position;

        DeathZoneObject.transform.position = new Vector3(deathZonePos.x,
                                                        platformPos.y-deathZoneGap,
                                                        deathZonePos.z);

        if (ballPos.y < platformPos.y-deathZoneGap){
            ResetBall();
        }

        // Trigger win
        if (GoalDetector.goalTrigger){
            GoalDetector.goalTrigger = false;
            MazeGenerator.HackingGameWin();
        }

        // if (Input.GetKeyDown("r") || DeathZone.fail){
        //     ResetBall();
        //     DeathZone.fail = false;
        // }


        
        if (Input.GetKey("left")){
            // print("left key was pressed");
            platform.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey("right")){
            // print("right key was pressed");
            platform.transform.Rotate(Vector3.back * speed * Time.deltaTime);
        }
        
        if (Input.GetKey("up")){
            // print("up key was pressed");
            platform.transform.Rotate(Vector3.right * speed * Time.deltaTime);
        }

        if (Input.GetKey("down")){
            // print("down key was pressed");
            platform.transform.Rotate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
