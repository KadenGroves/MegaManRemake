using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 20;
    [SerializeField] private int currentHealth = 20;
    [SerializeField] public Image healthBarImage; // Reference to the health bar image
    [SerializeField] public Sprite[] healthBarSprites; // Array to hold different health bar sprites
    [SerializeField] public int smallHealAmount = 2;
    [SerializeField] public int largeHealAmount = 8;
    public AudioSource audioSource;
    public AudioClip healthSound;
    public AudioClip deathSound;

    void Awake(){
        currentHealth = maxHealth;
    }

    void Start()
    {
        healthSound = Resources.Load<AudioClip>("Audio/refill");
        deathSound = Resources.Load<AudioClip>("Audio/death");
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            
        }

        UpdateHealthBar();

        if (currentHealth == 0)
        {
            // for some reason it breaks  being able to die. audioSource.PlayOneShot(deathSound); -Chessor
            Defeat();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        // for some reason it breaks picking up the healing orb audioSource.PlayOneShot(healthSound); - Chessor
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
    }

    void Defeat()
    {
        Debug.Log("Defeat.");
        SceneManager.LoadScene("Defeat");
    }

    public void HealingCapsule(string capsuleType)
    {
        switch (capsuleType)
        {
            case "Small":
                Heal(smallHealAmount);
                break;
            case "Large":
                Heal(largeHealAmount);
                break;
            default:
                Debug.LogWarning("Unknown capsule type!");
                break;
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp((currentHealth * healthBarSprites.Length) / maxHealth, 0, healthBarSprites.Length - 1);
            healthBarImage.sprite = healthBarSprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Occurred");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Collision with Enemy");
            var enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy is not null");
                TakeDamage(enemy.damage);
            }
            else
            {
                Debug.Log("Enemy is null, what're you DOING");
            }
        }
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public void SetCurrentHealth(int newHealthValue){
        currentHealth = newHealthValue;
        UpdateHealthBar();
    }
}