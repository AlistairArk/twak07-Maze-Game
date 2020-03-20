using UnityEngine;
using System.Collections;


public class Movement : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		UpdateMovement();
	}
	
	float speed = 8.0f;
    float jumpSpeed = 9.0f;
    float gravity = 30.0f;
    public static Vector3 moveDirection = Vector3.zero;
    public bool grounded = false;
    public bool CANJUMP = true;
	public bool UpHit;
 	public CharacterController controller;
	CollisionFlags flags;
	bool Jumping;
	float JumpPower;
    void UpdateMovement() {
        if (grounded) { Jumping = false; JumpPower = 0.0f;}

        if (grounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump")) {
				Jumping = true;
            }
        } else {
			moveDirection.x = Input.GetAxis("Horizontal") * speed;
		}
		/*	
		if(Jumping && Input.GetButton("Jump")) {
			if(JumpPower < jumpSpeed) {
				JumpPower += Time.deltaTime *50.0f;
				moveDirection.y = JumpPower;	
			} else {
				JumpPower = 0.0f;
				Jumping = false;
			}
		} else {
			JumpPower = 0.0f;
			Jumping = false;
		}
		*/
        moveDirection.y -= gravity * Time.deltaTime;
			
		flags = controller.Move(moveDirection * Time.deltaTime);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

    }

}

