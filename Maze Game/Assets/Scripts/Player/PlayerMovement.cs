using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    // public float mouseSensitivity = 2f;
    public LayerMask groundMask; 

    Vector3 velocity;
    bool isGrounded;
    public GameObject playerCamera;

    // Update is called once per frame
    void FixedUpdate() {

        // // Match cylinder and camera rotation
        // GameObject cube2 = gameObject;
        // float y = playerCamera.transform.rotation.y;
        // cube2.transform.rotation = Quaternion.Euler(cube2.transform.rotation.x, y, cube2.transform.rotation.z);
        // // .Rotate = new Vector3(transform.Rotate.x,playerCamera.transform.Rotate.y,transform.Rotate.z);


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y<0){
            velocity.y=-2f;
        }

        float x = Input.GetAxis("Horizontal"); // * mouseSensitivity * Time.deltaTime;
        float z = Input.GetAxis("Vertical"); // * mouseSensitivity * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
