using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Mono.Data.Sqlite;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class DatabaseController : MonoBehaviour
{
    string conn;
    private SqliteConnection dbconn;
    string sqlQuery;
    List<HighScore> highScores;
    public GameObject scoreboardPanel;
    private int mostRecentScore;
    
    string DATABASE_NAME = "/Database/mydatabase.s3db";

    private int loggedInUserId;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        highScores = new List<HighScore>();
        Debug.Log("Running start on database");

        string filepath;
#if UNITY_EDITOR
    filepath = Application.dataPath + DATABASE_NAME;
#else
    filepath = Application.persistentDataPath + DATABASE_NAME;
#endif

        Debug.Log($"Database location: {filepath}");

        string directoryPath = Path.GetDirectoryName(filepath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Created directory: " + directoryPath);
        }

        conn = "URI=file:" + filepath;

        try{
            CreateTable();
            if(SceneManager.GetActiveScene().name.Equals("LoginScene")){
                GetScores();
                ShowScores();
            }
        }
        catch (Exception e){
            Debug.LogError("Error initializing database: " + e.Message);
        }
    }

    public void DatabaseTestsSetup()
    {
        DontDestroyOnLoad(gameObject);
        highScores = new List<HighScore>();
        using(dbconn = new SqliteConnection(conn)){
            CreateTable();
            if(SceneManager.GetActiveScene().name.Equals("LoginScene")){
                GetScores();
                ShowScores();
            }
        }
    }

    private void CreateTable()
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();
            using (var dbcmd = dbconn.CreateCommand())
            {
                sqlQuery = "CREATE TABLE IF NOT EXISTS PlayerData (" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "username VARCHAR(255) NOT NULL UNIQUE," +
                    "password VARCHAR(255) NOT NULL," +
                    "isAdmin INTEGER NOT NULL DEFAULT 0);";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();

                sqlQuery = "CREATE TABLE IF NOT EXISTS PlayerScores (" +
                        "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "scoreById INTEGER NOT NULL, " +
                        "score INTEGER NOT NULL, " +
                        "FOREIGN KEY(scoreById) REFERENCES PlayerData(id)); ";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    private void GetScores()
    {
        Debug.Log("Running Get Scores Method");
        using (dbconn = new SqliteConnection(conn))
        {
            Debug.Log("Opening Database");
            dbconn.Open();
            using(var dbcmd = dbconn.CreateCommand()){
                Debug.Log("Database open");

                string sqlQuery = "SELECT PlayerData.username, PlayerScores.id, PlayerScores.score " +
                                "FROM PlayerScores INNER JOIN PlayerData ON PlayerScores.scoreById = PlayerData.id " +
                                "ORDER BY PlayerScores.score DESC LIMIT 10;";
                dbcmd.CommandText = sqlQuery;

                Debug.Log("Added Sqlite statement");

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    Debug.Log("Created reader to read database");
                    while (reader.Read())
                    {
                        string username = reader.GetString(0);
                        int score = reader.GetInt32(2);
                        highScores.Add(new HighScore(score, username));
                        Debug.Log("High Score added: Score of " + score + " by " + username);
                    }
                    Debug.Log("Closed Reader");
                }
            }

            Debug.Log("Close connection to database");
        }
    }

    private void ShowScores()
    {
        //Start index after title and headers
        int scoreStartIndex = 2;

        for (int i = 0; i < highScores.Count; i++)
        {
            // Access each score panel by index
            Transform scorePanel = scoreboardPanel.transform.GetChild(scoreStartIndex + i);

            if (scorePanel.childCount >= 3)
            {
                //Get the Name and the Score children
                TextMeshProUGUI nameText = scorePanel.GetChild(1).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scoreText = scorePanel.GetChild(2).GetComponent<TextMeshProUGUI>();

                // Update the Name and Score fields
                if (nameText != null && scoreText != null)
                {
                    nameText.text = highScores[i].username;
                    scoreText.text = highScores[i].score.ToString();
                    Debug.Log($"Updated UI: Username: {highScores[i].username}, Score: {highScores[i].score}");
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI components not found in expected child objects.");
                }
            }
            else
            {
                Debug.LogWarning($"Score panel {i} does not contain the expected number of children.");
            }
        }
    }

    public bool CreateUser(string username, string password)
    {
        Debug.Log("Attempting User Creation");
        try{
            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open();
                using(var dbcmd = dbconn.CreateCommand()){
                    string sqlQuery = "INSERT INTO PlayerData (username, password) VALUES (@username, @password);";
                    dbcmd.CommandText = sqlQuery;

                    var usernameParam = dbcmd.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbcmd.Parameters.Add(usernameParam);

                    var passwordParam = dbcmd.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = HashPassword(password);
                    dbcmd.Parameters.Add(passwordParam);

                    dbcmd.ExecuteNonQuery();

                    dbconn.Close();
                }
            }
        }
        catch(SqliteException ex){
            if (ex.ErrorCode == SQLiteErrorCode.Constraint)
            {
                Debug.Log("Username already exists in the database.");
                return false;
            }
        }
        catch(Exception e){
            Debug.Log(e);
            return false;
        }

        return true;
    }

    public bool CreateAdminUser(string username, string password)
    {
        Debug.Log("Attempting User Creation");
        try{
            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open();
                using(var dbcmd = dbconn.CreateCommand()){
                    string sqlQuery = "INSERT INTO PlayerData (username, password, isAdmin) VALUES (@username, @password, 1);";
                    dbcmd.CommandText = sqlQuery;

                    var usernameParam = dbcmd.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbcmd.Parameters.Add(usernameParam);

                    var passwordParam = dbcmd.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = HashPassword(password);
                    dbcmd.Parameters.Add(passwordParam);

                    dbcmd.ExecuteNonQuery();
                }
                dbconn.Close();
            }
        }
        catch(SqliteException ex){
            if (ex.ErrorCode == SQLiteErrorCode.Constraint)
            {
                Debug.Log("Username already exists in the database.");
                return false;
            }
        }
        catch(Exception e){
            Debug.Log(e);
            return false;
        }

        return true;
    }

    public bool LoginUser(string username, string password)
    {
        Debug.Log("Attempting User Login");
        try
        {
            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open();
                using(var dbcmd = dbconn.CreateCommand()){
                    string sqlQuery = "SELECT id FROM PlayerData WHERE username = @username AND password = @password;";
                    dbcmd.CommandText = sqlQuery;

                    var usernameParam = dbcmd.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbcmd.Parameters.Add(usernameParam);

                    var passwordParam = dbcmd.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = HashPassword(password);
                    dbcmd.Parameters.Add(passwordParam);

                    using (IDataReader reader = dbcmd.ExecuteReader())
                    {
                        if (reader.Read())  // Check if there is at least one result
                        {
                            loggedInUserId = reader.GetInt32(0);
                            Debug.Log($"Login successful. User ID: {loggedInUserId}");
                            return true;
                        }
                        else
                        {
                            Debug.Log("Username and Password not found in the database");
                            return false;
                        }
                    }
                }
            }
        }
        catch (SqliteException ex)
        {
            Debug.Log("Failure when interacting with SQLite Database: " + ex);
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2")); // Convert byte to hex
            }
            return builder.ToString();
        }
    }

    public bool IsAdmin()
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();
            using(var dbcmd = dbconn.CreateCommand()){
                string sqlQuery = "SELECT isAdmin FROM PlayerData WHERE id = @userId;";
                dbcmd.CommandText = sqlQuery;

                var idParam = dbcmd.CreateParameter();
                idParam.ParameterName = "@userId";
                idParam.Value = loggedInUserId;
                dbcmd.Parameters.Add(idParam);

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int isAdmin = reader.GetInt32(0);
                        return isAdmin == 1;
                    }
                }
            }
        }
        return false;
    }

    public void SaveScore(int score){
        using (dbconn = new SqliteConnection(conn)){
            dbconn.Open();
            using(var dbcmd = dbconn.CreateCommand()){
                string sqlQuery = "INSERT INTO PlayerScores (scoreById, score) VALUES (@scoreById, @score);";
                dbcmd.CommandText = sqlQuery;

                var scoreByIdParam = dbcmd.CreateParameter();
                scoreByIdParam.ParameterName = "@scoreById";
                scoreByIdParam.Value = loggedInUserId;
                dbcmd.Parameters.Add(scoreByIdParam);

                var scoreParam = dbcmd.CreateParameter();
                scoreParam.ParameterName = "@score";
                scoreParam.Value = score;
                dbcmd.Parameters.Add(scoreParam);

                dbcmd.ExecuteNonQuery();
            }
        }

        mostRecentScore = score;
    }

    //FOR TESTING PURPOSES ONLY
    public bool CheckForScoreInDatabase(int score){
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();
            using (var dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT score FROM PlayerScores WHERE scoreById = @userId AND score = @score LIMIT 1;";
                dbcmd.CommandText = sqlQuery;

                var idParam = dbcmd.CreateParameter();
                idParam.ParameterName = "@userId";
                idParam.Value = loggedInUserId;
                dbcmd.Parameters.Add(idParam);

                var scoreParam = dbcmd.CreateParameter();
                scoreParam.ParameterName = "@score";
                scoreParam.Value = score;
                dbcmd.Parameters.Add(scoreParam);

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int scoreInDb = reader.GetInt32(0);
                        Debug.Log($"Score found in DB: {scoreInDb}");
                        return scoreInDb == score;
                    }
                }
            }

            return false;
        }
    }

    public void CloseDatabaseConnection()
    {
        if (dbconn != null)
        {
            if (dbconn.State == ConnectionState.Open)
            {
                dbconn.Close();
                Debug.Log("Database connection closed.");
            }
            dbconn.Dispose();
            dbconn = null;
        }
    }

    //FOR TESTING PURPOSES ONLY
    public void SetDatabasePath(string customPath)
    {
        conn = "URI=file:" + customPath;
    }

    public int GetMostRecentScore(){
        return mostRecentScore;
    }

    public void logout(){
        loggedInUserId = 0;
        mostRecentScore = 0;
    }

    public int GetCurrentUserId(){
        return loggedInUserId;
    }
}

class HighScore{
    public int score = 0;
    public string username;

    public HighScore(int score, string username)
    {
        this.score = score;
        this.username = username;
    }
}
