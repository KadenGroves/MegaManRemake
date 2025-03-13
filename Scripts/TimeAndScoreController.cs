using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class TimeAndScoreController : MonoBehaviour
{
    public TextMeshProUGUI timerText, scoreText;    
    public float timeRemaining = 600f; 
    public int score = 0;
    private bool isGameOver = false;

    void Start(){
        Debug.Log("Score and Timer running");
        scoreText.text = string.Format("{0:0000}", score);
    }

    void Update()
    {
        if (!isGameOver)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isGameOver = true;
                HandleDefeat();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void HandleDefeat()
    {
        Debug.Log("Time's up! Player defeated.");
        SceneManager.LoadScene("Defeat");
    }

    public void UpdateScore(int addedPoints){
        score += addedPoints;
        scoreText.text = string.Format("{0:0000}", score);
    }

    public void AddTimeToScore(){
        score += (int)timeRemaining * 10;
    }
}
