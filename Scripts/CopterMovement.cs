using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopterMovement : MonoBehaviour
{
 public float speed = 5f; // Speed at which the enemy flies toward the player
    public Transform player; // Reference to the player's transform
    public float detectionRange = 10f; // Distance to detect the player

    private bool playerInView = false;

    void Update()
    {
        // Check if the player is within detection range and visible in the camera view
        if (player != null && IsPlayerInView() && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            playerInView = true;
        }
        else
        {
            playerInView = false;
        }

        // If the player is in view, move toward the player
        if (playerInView)
        {
            FlyTowardPlayer();
        }
    }

    private bool IsPlayerInView()
    {
        // Convert player position to viewport position (0 to 1 on screen)
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(player.position);

        // Check if the player is within the camera's view (x and y between 0 and 1, and z > 0 for in front of the camera)
        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }

    private void FlyTowardPlayer()
    {
        // Calculate direction toward the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy toward the player
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection range for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}