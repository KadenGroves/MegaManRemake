using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        int finalScore = GameObject.Find("DatabaseController").GetComponent<DatabaseController>().GetMostRecentScore();
        finalScoreText.text = $"Final Score: {finalScore}";
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
