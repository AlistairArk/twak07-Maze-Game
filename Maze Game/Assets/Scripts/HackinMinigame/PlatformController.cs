using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour{

    public float speed = 10f;
    
    public GameObject ball; 
    public GameObject platform;
    public GameObject DeathZoneObject;
    public int deathZoneGap = 50;


    private DeathZone DeathZone;

    // Start is called before the first frame update
    void Awake(){
        DeathZone = DeathZoneObject.GetComponent<DeathZone>();
    }

    void ResetBall(){
        ball.transform.localPosition = new Vector3(0f, 10f, 0f);
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

        // if (Input.GetKeyDown("r") || DeathZone.fail){
        //     ResetBall();
        //     DeathZone.fail = false;
        // }


        
        if (Input.GetKey("left")){
            print("left key was pressed");
            platform.transform.Rotate(Vector3.forward * speed  * Time.deltaTime);
        }

        if (Input.GetKey("right")){
            print("right key was pressed");
            platform.transform.Rotate(Vector3.back * speed  * Time.deltaTime);
        }
        
        if (Input.GetKey("up")){
            print("up key was pressed");
            platform.transform.Rotate(Vector3.right * speed  * Time.deltaTime);
        }

        if (Input.GetKey("down")){
            print("down key was pressed");
            platform.transform.Rotate(Vector3.left * speed  * Time.deltaTime);
        }
    }
}
