using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//CHESSOR 10.18.24
public class MainMenu : MonoBehaviour
{
    public GameObject editDatabaseButton;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if(GameObject.Find("DatabaseController").GetComponent<DatabaseController>().IsAdmin()){
            editDatabaseButton.SetActive(true);
        }
        else{
            editDatabaseButton.SetActive(false);
        }
    }
    public void StartGame()
    {

        SceneManager.LoadScene("MainScene");
        Debug.Log("MAINSCENE LOADED.");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGame()
    {
        Application.Quit();

        //If Unity Editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void Logout()
    {
        GameObject.Find("DatabaseController").GetComponent<DatabaseController>().logout();
        SceneManager.LoadScene("LoginScene");
    }

    public void LoadDatabaseEditor()
    {
        SceneManager.LoadScene("DatabaseEditor");
    }

    public void ShowCredits(){
        SceneManager.LoadScene("Credits");
    }
}
