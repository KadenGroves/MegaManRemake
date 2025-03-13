using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SceneTimeTracker : MonoBehaviour
{
    private float startTime;
    private Dictionary<string, float> sceneTimes = new Dictionary<string, float>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Start()
    {
        startTime = Time.time;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startTime = Time.time;  // Reset start time for the new scene
    }

    void OnSceneUnloaded(Scene scene)
    {
        LogSceneTime(); // Log time when leaving a scene
    }

    void LogSceneTime()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        float elapsedTime = Time.time - startTime;

        if (elapsedTime < 0.1f) return;  // Skip logging if elapsed time is too short

        if (sceneTimes.ContainsKey(sceneName))
            sceneTimes[sceneName] += elapsedTime;
        else
            sceneTimes[sceneName] = elapsedTime;

        startTime = Time.time;  // Reset timer after logging
    }

    void OnApplicationQuit()
    {
        LogSceneTime();  // Final log on application quit

        // Log to Console
        Debug.Log("Scene Times:");
        foreach (var entry in sceneTimes)
        {
            Debug.Log($"Scene: {entry.Key}, Time Spent: {entry.Value:F2} seconds");
        }

        // Log to File
        string filePath = Path.Combine(Application.persistentDataPath, "SceneTimeLog.txt");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Scene Times:");
            foreach (var entry in sceneTimes)
            {
                writer.WriteLine($"Scene: {entry.Key}, Time Spent: {entry.Value:F2} seconds");
            }
        }
        Debug.Log($"Scene time log saved to: {filePath}");
    }
}