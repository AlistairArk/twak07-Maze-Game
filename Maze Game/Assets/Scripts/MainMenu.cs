using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour{

    [Header("Menu", order=0)]

    public GameObject menuUI;
    public GameObject cam2Object;
    private Camera cam2;


    [Header("In Game", order=1)]
    public GameObject cam1Object;
    public GameObject gameUI;
    private Camera cam1;

    [Header("Misc.", order=2)]
    public bool showMenu = true;

    void Start() {
        cam1 = cam1Object.GetComponent<Camera>();
        cam2 = cam2Object.GetComponent<Camera>();
        cam1.enabled = true;
        cam2.enabled = false;
    }


    void Update() {
        switch(showMenu){
            case(true): // Menu
            cam2.enabled = true;
            cam1.enabled = false;
            gameUI.SetActive(false);
            menuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            break;

            case(false): // Game
            cam1.enabled = true;
            cam2.enabled = false;
            gameUI.SetActive(true);
            menuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            break;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            showMenu =!showMenu;
        }
    }



    public void ButtonPlayGame(){
        showMenu =!showMenu;
    } 
}


