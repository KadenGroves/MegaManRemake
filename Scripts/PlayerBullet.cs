using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private GameObject weakBullet;
    [SerializeField] private GameObject strongBullet;
    [SerializeField] private GameObject chargedBullet;
    private string bulletType;
    private Animator anim;

    //CHESSOR 11.19.24
    public AudioSource audioSource;
    public AudioClip fireSoundWeak;
    public AudioClip fireSoundCharged;
    public AudioClip fireSoundStrong;

    private void Awake(){
        //CHESSOR 11.19.24
        fireSoundWeak = Resources.Load<AudioClip>("Audio/buster");
        fireSoundCharged = Resources.Load<AudioClip>("Audio/strongbuster");
        fireSoundStrong = Resources.Load<AudioClip>("Audio/mediumbuster");
        audioSource = gameObject.AddComponent<AudioSource>();
        anim = transform.GetComponent<Animator>();
    }

    public void MakeBulletType(string type, float playerFacing){
        if(type.Equals("Weak")){
            bulletType = "Weak";
            transform.localScale = weakBullet.GetComponent<Transform>().localScale;
            transform.GetComponent<BoxCollider2D>().size = weakBullet.GetComponent<BoxCollider2D>().size;
            transform.localScale = new Vector3(playerFacing*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            audioSource.PlayOneShot(fireSoundWeak);
            anim.SetTrigger("WeakBullet");
        }
        else if(type.Equals("Strong")){
            bulletType = "Strong";
            transform.localScale = strongBullet.GetComponent<Transform>().localScale;
            transform.GetComponent<BoxCollider2D>().size = strongBullet.GetComponent<BoxCollider2D>().size;
            transform.localScale = new Vector3(playerFacing*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            audioSource.PlayOneShot(fireSoundStrong);
            anim.SetTrigger("StrongBullet");
        }
        else if(type.Equals("Charged")){
            bulletType = "Charged";
            transform.localScale = chargedBullet.GetComponent<Transform>().localScale;
            transform.GetComponent<BoxCollider2D>().size = chargedBullet.GetComponent<BoxCollider2D>().size;
            transform.localScale = new Vector3(playerFacing*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            audioSource.PlayOneShot(fireSoundCharged);
            anim.SetTrigger("ChargedBullet");
        }
    }
    public int GetDamage(){
        int damage = 0;

        if(bulletType.Equals("Weak")){
            damage = 1;
        }
        else if(bulletType.Equals("Strong")){
            damage = 4;
        }
        else if(bulletType.Equals("Charged")){
            damage = 10;
        }

        return damage;
    }
    public float GetSpeed(){
        float speed = 0f;

        if(bulletType.Equals("Weak")){
            speed = 8f;
        }
        else if(bulletType.Equals("Strong")){
            speed = 10f;
        }
        else if(bulletType.Equals("Charged")){
            speed = 12f;
        }
        
        return speed;
    }

    public void Explode(){
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        anim.SetTrigger("Explode");
    }
}