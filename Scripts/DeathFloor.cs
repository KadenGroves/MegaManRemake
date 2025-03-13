using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  
        {
            HandleDefeat();
        }
    }

    void HandleDefeat()
    {
        Debug.Log("Player fell off the map! Game Over.");
        SceneManager.LoadScene("Defeat");
    }
}
