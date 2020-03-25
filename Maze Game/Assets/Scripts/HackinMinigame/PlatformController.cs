using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour{

    public float speed = 10f;
    
    public GameObject ball; 
    public GameObject platform; 
    public DeathZone DeathZone;

    // Start is called before the first frame update
    void Start(){
        
    }

    void ResetBall(){
        ball.transform.localPosition = new Vector3(25f, 10f, 25f);
    }


    void Update()    {
        if (Input.GetKeyDown("r")){
            ResetBall();
        }
        
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
