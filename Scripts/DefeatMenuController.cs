using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("MAIN SCENE LOADED.");
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MAIN MENU LOADED");
    }
}
