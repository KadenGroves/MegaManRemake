using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public int damage = 2;
    public int pointValue = 100;
    private bool isDead = false;

    //CHESSOR 11.19.24
    public AudioSource audioSource;
    public AudioClip hitSound;

    public GameObject smallHealthCapsulePrefab; 
    public GameObject largeHealthCapsulePrefab; 

    void Start()
    {
        //CHESSOR 11.19.24
        hitSound = Resources.Load<AudioClip>("Audio/enemy_hit");
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        audioSource.PlayOneShot(hitSound);
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            Die();
        }
    }

    private void Die()
    {
        
        if (isDead) return; 
        isDead = true;

      
        GameObject.Find("TimeAndScoreController").GetComponent<TimeAndScoreController>().UpdateScore(pointValue);
        Debug.Log("Enemy defeated! Worth " + pointValue + " points!");

     
        SpawnHealthCapsule();

        //Destroys the visuals of the enemy
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        Destroy(gameObject.GetComponent<CircleCollider2D>());

        StartCoroutine(PlayDeathSoundAndDestroy());
    }

    private void SpawnHealthCapsule()
    {
       
        int randomNumber = Random.Range(1, 10);


        if (randomNumber >= 4 && randomNumber <= 6)
        {

            Instantiate(smallHealthCapsulePrefab, transform.position, Quaternion.identity);
            Debug.Log("Small health capsule spawned.");
        }
        else if (randomNumber >= 7 && randomNumber <= 9)
        {

            Instantiate(largeHealthCapsulePrefab, transform.position, Quaternion.identity);
            Debug.Log("Large health capsule spawned.");
        }
        else
        {
            Debug.Log("No health capsule spawned.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("Bullet collided with Enemy");
            
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();
            if (bullet != null)
            {
                int bulletDamage = bullet.GetDamage();
                Debug.Log("Bullet damage: " + bulletDamage); 
                TakeDamage(bulletDamage);
                bullet.Explode();
                Debug.Log("Waiting for explosion animation");
                StartCoroutine(DestroyAfterExplosion(other.gameObject, 0.25f));
            }
        }
    }

    private IEnumerator DestroyAfterExplosion(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Explosion animation is over");
        Destroy(bullet);
    }
    private IEnumerator PlayDeathSoundAndDestroy()
    {
        audioSource.PlayOneShot(hitSound);

        //Wait for the audio to finish playing
        yield return new WaitForSeconds(hitSound.length);

        //Fully destroy the GameObject
        Destroy(gameObject);
    }
}