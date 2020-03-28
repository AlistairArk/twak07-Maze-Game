using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    [Header("Settings", order=0)]
    public bool enableVR = false;

    [Header("Player Movement", order=1)]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    [Header("Arguments", order=2)]
    public CharacterController controller;
    public GameObject playerCamera;
    public GameObject xrCam;
    public LayerMask groundMask; 

    [Header("Misc.", order=3)]
    public Transform groundCheck;
    public Vector3 velocity;
    public bool isGrounded;
    public GameObject activeCam;

    void Awake(){
        
        // if (enableVR){
        //     xrCam.SetActive(true);
        //     playerCamera.SetActive(false);
        //     activeCam = xrCam;
        // }else{
        //     xrCam.SetActive(false);
        //     playerCamera.SetActive(true);
        //     activeCam = playerCamera;
        // }
    }


    // Update is called once per frame
    void FixedUpdate() {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y<0){
            velocity.y=-2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

/*
//Movement Stuff(WASD)
Vector3 movement = Vector3.zero;
float v = Input.GetAxis(PlayerInput.Vertical);
float h = Input.GetAxis(PlayerInput.Horizontal);
movement += transform.forward * v * moveSpeed * Time.deltaTime;
movement += transform.right * h * moveSpeed * Time.deltaTime;
movement += Physics.gravity;
_cc.Move(movement);
*/