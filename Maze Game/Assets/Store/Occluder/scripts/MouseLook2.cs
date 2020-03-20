using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 0.0f;
    public float sensitivityY = 0.0f;

    public float minimumX = -360f;
    public float maximumX = 360f;

    public float  minimumY = -70f;
    public float maximumY = 70f;

    float rotationX = 0;
    float rotationY = 0;
	
	Quaternion yQuaternion;
    Quaternion xQuaternion;

    Quaternion originalRotation;

    //private GameLocal gamelocal;
	
	bool Shake;
	float ShakeStrength, ShakeLength;

	void Start () {
        //gamelocal = GameObject.Find("GameLocal").GetComponent<GameLocal>();
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        originalRotation = transform.localRotation;
	}
	
	public void AddRecoil(float recoil) {
		rotationY += recoil;
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.localRotation = originalRotation * yQuaternion;
	}
	
	public void AddCameraShake(float Strength, float Length) {
		Shake = true;
		ShakeStrength = Strength;
		ShakeLength = Time.time + Length;
	}
	
	// Update is called once per frame
	void Update () {
		//if(gamelocal.IsPlayerAlive) {
	        sensitivityX = 3;
	        sensitivityY = 3;
	        if (!Screen.lockCursor) {
	            //return;
	        }
	
	        if (axes == RotationAxes.MouseXAndY) {
	            // Read the mouse input axis
	            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
	            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
	
	            rotationX = ClampAngle(rotationX, minimumX, maximumX);
	            rotationY = ClampAngle(rotationY, minimumY, maximumY);
	
	            xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
	            yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
	
	            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
	        } else if (axes == RotationAxes.MouseX) {
	            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
	            rotationX = ClampAngle(rotationX, minimumX, maximumX);
	
	            xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
	            transform.localRotation = originalRotation * xQuaternion;
	       } else {
	            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
	            rotationY = ClampAngle(rotationY, minimumY, maximumY);
	
	            yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
	            transform.localRotation = originalRotation * yQuaternion;
	        }
			
			if(Shake) {
				if(Time.time < ShakeLength) {
					rotationX += Random.Range(-ShakeStrength, ShakeStrength);
	            	rotationY += Random.Range(-ShakeStrength, ShakeStrength);
	            	rotationX = ClampAngle(rotationX, minimumX, maximumX);
	            	rotationY = ClampAngle(rotationY, minimumY, maximumY);
	            	xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
	            	yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
					transform.localRotation = originalRotation * xQuaternion * yQuaternion;
				} else {
					Shake = false;
					ShakeStrength = 0.0f;
					ShakeLength = 0.0f;
				}
			}
		//}
    }

    static float ClampAngle ( float angle, float min, float max) {
	    if (angle < -360)
		    angle += 360;
	    if (angle > 360)
		    angle -= 360;
	    return Mathf.Clamp (angle, min, max);
    }

}
