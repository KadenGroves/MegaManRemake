using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCapsule : MonoBehaviour
{
    public string capsuleType; // "Small" or "Large"

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.HealingCapsule(capsuleType);
                Destroy(gameObject); // Destroy the capsule after healing
            }
        }
    }
}
