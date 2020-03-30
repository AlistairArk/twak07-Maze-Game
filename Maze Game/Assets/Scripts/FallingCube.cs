using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCube : MonoBehaviour{


    Vector3 rotationAxis;

    int maxHeight = 100;   // Cube - Max spawn hight - On initialization
    int minHeight =-100;   // Cube - Min spawn hight - On initialization
    int gridX = 50;        // Size of the map grid - X
    int gridZ = 50;        // Size of the map grid - Z
    

    float size=50f;         // Size of the cubefield
    float perimeter=3f;     // Gap between cubefield and the platform 
    int deadZone = -100;    // Point at whcich the cube respawns

    public float speed=0.5f;// Speed at which the cube will rotate
    public Vector3 pos;     // Position of the cube
    public Vector3 rot;     // Vector in which the cubes will rotate

    // Start is called before the first frame update
    void Start(){
        // rotationAxis = new Vector3(,,);   
        int chance = Random.Range(0, 4);
        
        float spawnX = 0;
        float spawnZ = 0;

        // Select side of platform from which to spawn the cube
        if (chance==0){  // North field
            spawnX = Random.Range(-size, gridX+size);
            spawnZ = Random.Range(gridZ+perimeter, size+gridZ+perimeter);

        }else if(chance==1){ // East Field
            spawnX = Random.Range(gridX+perimeter, gridX+size+perimeter);
            spawnZ = Random.Range(-size, size+gridZ);

        }else if(chance==2){ // South Field
            spawnX = Random.Range(-size, gridX+size);
            spawnZ = Random.Range(-size-perimeter, -perimeter);

        }else if(chance==3){ // West field
            spawnX = Random.Range(-size-perimeter, -perimeter);
            spawnZ = Random.Range(-size, gridZ+size);
        }

        // Set random position of the cube
        Vector3 position = new Vector3(spawnX, (float)Random.Range(maxHeight, minHeight), spawnZ); 
        transform.localPosition = position;

        // Set random rotation vector
        rot = new Vector3(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update(){
        pos = transform.localPosition;

        // Move back to top
        if (pos.y<deadZone) transform.localPosition = new Vector3(pos.x, maxHeight, pos.z);

        // Rotate cube
        transform.Rotate(rot * speed * Time.deltaTime);
    }
}
