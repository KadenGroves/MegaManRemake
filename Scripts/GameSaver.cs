using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private int currentUserId;
    private string filePath, directoryPath;
    TimeAndScoreController timeAndScoreController;
    PlayerHealth playerHealth;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject checkpoint;
    void Start(){
        currentUserId = GameObject.Find("DatabaseController").GetComponent<DatabaseController>().GetCurrentUserId();
        timeAndScoreController = GameObject.Find("TimeAndScoreController").GetComponent<TimeAndScoreController>();
        playerHealth = GameObject.Find("Mega Man X").GetComponent<PlayerHealth>();

        directoryPath = Path.Combine(Application.dataPath, "PlayerSaves");
        filePath = Path.Combine(directoryPath, $"playersaveid{currentUserId}.txt");

        //Ensure the directory exists, if not it creates the directory
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        //Check to see if there is a file save for the user id
        if(File.Exists(filePath)){
            LoadGame();
        }
    }
    public void SaveGame()
    {
        checkpoint.GetComponent<BoxCollider2D>().enabled = false;

        // Retrieve necessary components and data
        float currentTimeLeft = timeAndScoreController.timeRemaining;
        int currentScore = timeAndScoreController.score;
        int currentHealth = playerHealth.GetCurrentHealth();

        Debug.Log($"Directory path is: {directoryPath}");
        Debug.Log($"File path is: {filePath}");

        // Save data to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(currentTimeLeft);
            writer.WriteLine(currentScore);
            writer.WriteLine(currentHealth);
        }

        Debug.Log("Game saved to " + filePath);
    }

    public void LoadGame(){
        checkpoint.GetComponent<BoxCollider2D>().enabled = false;

        float currentTimeRemaining = 0f;
        int currentScore = 0;
        int currentHealth = 0;

        using (StreamReader reader = new StreamReader(filePath))
        {
            if (float.TryParse(reader.ReadLine(), out float timeRemaining)){
                currentTimeRemaining = timeRemaining;
            }
            if (int.TryParse(reader.ReadLine(), out int score)){
                currentScore = score;
            }
            if (int.TryParse(reader.ReadLine(), out int health)){
                currentHealth = health;
            }
        }

        timeAndScoreController.timeRemaining = currentTimeRemaining;
        timeAndScoreController.score = currentScore;
        playerHealth.SetCurrentHealth(currentHealth);

        //Destroy the first 16 enemies in the game that come before the checkpoint when loading the game
        for(int i = 0; i<=15; i++){
            Destroy(enemies[i]);
        }

        //Move Player and Camera to the checkpoint
        Transform playerTransform = GameObject.Find("Mega Man X").GetComponent<Transform>();
        Transform cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();

        playerTransform.position = new Vector3(170.6f, 0.76f, -5f);
        cameraTransform.position = new Vector3(170.6f, 0f, -10f);
    }

    public void DeleteCheckpointSave(){
        if(File.Exists(filePath)){
            File.Delete(filePath);
        }
    }
}
