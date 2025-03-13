using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissileProjectile : MonoBehaviour
{
    public float speed = 12f; 
    public float lifetime = 3f; 

    private Vector2 direction; 

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); 
    }

    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(4); 
            Destroy(gameObject); 
        }
    }
}