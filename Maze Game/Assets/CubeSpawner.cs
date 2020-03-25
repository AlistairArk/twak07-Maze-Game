using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour{

    public GameObject cube;
    public int spawnCount = 40;

    // Start is called before the first frame update
    void Start(){
        int i=0;
        while (i!=spawnCount){
            i++;
            // CreatePrefab(cube, MazeGlobals.cellDoorParent, new Vector3(X+.8f, 0f, Z+.4f), new Vector3(1,1,1), 270);
            // cube.transform.localScale = new Vector3(scale.x,scale.y,scale.z);
            // int mapScale = MazeGlobals.mapScale;

            GameObject newCube;
            newCube = Instantiate(cube); // , new Vector3(pos.x*mapScale, pos.y, pos.z*mapScale), Quaternion.Euler(0,rotation,0));
            newCube.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


        

}
