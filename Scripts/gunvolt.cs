using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunvolt : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform player; // Reference to the player's transform
    public float attackRange = 10f; // Range at which Gunvolt will attack
    public float fireRate = 1f; // Rate of fire for projectiles

    private float nextFireTime = 5f;

    private void Update()
    {
        // Check distance to player
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Check if Gunvolt can fire a projectile based on the fire rate
        if (Time.time >= nextFireTime)
        {
            // Create a projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Set direction of the projectile towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            projectile.GetComponent<MissileProjectile>().SetDirection(direction);

            // Update the next fire time
            nextFireTime = Time.time + fireRate;
        }
    }
}