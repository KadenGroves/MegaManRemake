using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaver : MonoBehaviour
{
    public void SaveScoreToDatabase(){
        GameObject.Find("GameSaver").GetComponent<GameSaver>().DeleteCheckpointSave();
        TimeAndScoreController timeAndScoreController = GameObject.Find("TimeAndScoreController").GetComponent<TimeAndScoreController>();
        timeAndScoreController.AddTimeToScore();
        int finalScore = timeAndScoreController.score;
        GameObject.Find("DatabaseController").GetComponent<DatabaseController>().SaveScore(finalScore);
    }
}
