using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    public float m_speed = 5f;
    public float jumpforce = 10f;
    private float wallJumpCooldown;
    private float horizontalInput;

    public AudioSource audioSource;
    public AudioClip collectSound;
    public AudioClip landSound;
    //public GameObject bulletPrefab;

    //public AudioClip collectSound;

    void Awake()
    {
        landSound = Resources.Load<AudioClip>("Audio/land");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        m_Rigidbody.velocity = new Vector2(horizontalInput * m_speed, m_Rigidbody.velocity.y);

        if (horizontalInput > 0.01f){
            transform.localScale = new Vector3(4.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < -0.01f){
            transform.localScale = new Vector3(-4.5f, transform.localScale.y, transform.localScale.z);
        }

        //Wall jump implementation and checks
        if(wallJumpCooldown > 0.2f){
            m_Rigidbody.velocity = new Vector2(horizontalInput * m_speed, m_Rigidbody.velocity.y);

            if(onWall() && !isGrounded()){
                m_Rigidbody.gravityScale = 0;
                m_Rigidbody.velocity = Vector2.zero;
            }
            else
                m_Rigidbody.gravityScale = 1.75f;
        
            if (Input.GetKeyDown(KeyCode.X))
            {
                Jump();
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());

        //if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        //{
            //footstepSource.Play();
            //footstepSource.loop = true;
        //}

        //if(!Input.anyKey)
        //{
            //footstepSource.loop = false;
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
            //Fire();
        //}
    }

    private void Fire()
    {
        //GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    private void Jump()
    {
        if(isGrounded()){
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, jumpforce);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded()){
            if(horizontalInput == 0){
                m_Rigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, jumpforce/4);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * 4.5f, transform.localScale.y, transform.localScale.z);
            }
            else
                m_Rigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 20, jumpforce/2);

            wallJumpCooldown = 0f;
        }
    }

    private bool isGrounded(){
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

    private bool onWall(){
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit2D.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Checkpoint")){
            GameObject.Find("GameSaver").GetComponent<GameSaver>().SaveGame();
        }
        if(other.gameObject.tag.Equals("Finish")){
            other.GetComponent<ScoreSaver>().SaveScoreToDatabase();
            SceneManager.LoadScene("Victory");
            Debug.Log("MAIN MENU LOADED");
        }
    }
}
