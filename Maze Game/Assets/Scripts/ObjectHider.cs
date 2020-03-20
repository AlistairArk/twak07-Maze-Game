using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour{
    // Disable the behaviour when it becomes invisible...
    void OnBecameInvisible(){
        print("Lost");
        foreach (Transform child in transform)
            child.transform.gameObject.SetActive(false);
    }

    // ...and enable it again when it becomes visible.
    void OnBecameVisible(){
        print("Found");
        foreach (Transform child in transform)
            child.transform.gameObject.SetActive(true);
    }
}
