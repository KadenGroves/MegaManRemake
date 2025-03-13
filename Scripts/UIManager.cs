using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseScreen.activeInHierarchy){
                PauseGame(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                PauseGame(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void PauseGame(bool status){
        pauseScreen.SetActive(status);

        if(status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void ExitToMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MAIN MENU LOADED");
    }
}
