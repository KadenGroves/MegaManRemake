using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private float cooldownTimer = 10f; //Allows player to immeadiately fire when loaded into game without waiting for the cooldown on game start.
    [SerializeField] private float chargeTime = 0f;
    private bool chargingBuster = false;
    [SerializeField] private GameObject PlayerBullet;

    public AudioSource audioSource;
    public AudioClip chargingSound;
    private void Awake()
    {
        audioSource = new AudioSource();
        chargingSound = Resources.Load<AudioClip>("Audio/charging");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && cooldownTimer >= attackCooldown)
        {
            
            chargingBuster = true;
            chargeTime = 0f;
            anim.SetBool("Charging", true);
            //audioSource.PlayOneShot(chargingSound);
        }
        if (Input.GetKey(KeyCode.C) && chargingBuster)
        {
           
            chargeTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.C) && anim.GetBool("Charging"))
        {
            chargingBuster = false;
            Shoot();
            anim.SetTrigger("Fire");
            anim.SetBool("Charging", false);
        }
    }

    private void Shoot()
    {
        Vector3 bulletPositionOffset = new Vector3(Mathf.Sign(transform.localScale.x) * 0.85f, 0.06f, 0);

        GameObject bullet = Instantiate(PlayerBullet, transform.position + bulletPositionOffset, Quaternion.identity);

        float playerFacing = Mathf.Sign(transform.localScale.x);

        if (chargeTime <= 0.5f)
        {
            bullet.GetComponent<PlayerBullet>().MakeBulletType("Weak", playerFacing);
        }
        else if (chargeTime > 0.5f && chargeTime <= 1.5f)
        {
            bullet.GetComponent<PlayerBullet>().MakeBulletType("Strong", playerFacing);
        }
        else if (chargeTime > 1.5f)
        {
            bullet.GetComponent<PlayerBullet>().MakeBulletType("Charged", playerFacing);
        }

        float projSpeed = bullet.GetComponent<PlayerBullet>().GetSpeed();
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(playerFacing*projSpeed, 0);

        cooldownTimer = 0;
    }
}
