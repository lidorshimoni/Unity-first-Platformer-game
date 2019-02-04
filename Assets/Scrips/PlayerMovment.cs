using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class PlayerMovment : MonoBehaviour
{
    public int count = 0;
    public float speed;
    public float ladderSpeed;
    public float ladderDistance;
    public float jumpForce;

    private bool isClimbing;

    private float horizontalMoveInput;
    private float verticalMoveInput;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;

    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public LayerMask whatIsEnemy;

    private int extraJumps;
    public int extraJumpsValue;

    private SpriteRenderer sr;

    private bool justLanded;
    public GameObject dustpuff;

    public Transform attackPoint;
    public GameManager game;

    public float startTimeBtwAttack;
    private float timeBtwAttack;
    public Transform attackPos;
    public float attackRangeX;
    public float attackRangeY;
    public float damage;

    private float timeBtwEchoSpawns;
    public float startTimeBtwEchoSpawns;
    public GameObject echoEffect;


//setup
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
    }

//physics
    void FixedUpdate()
    {
        // gets input
        horizontalMoveInput = Input.GetAxis("Horizontal");
        verticalMoveInput = Input.GetAxisRaw("Vertical");

        //moves according to horizontal input
        move();

        //checks if the player is touching the ground
        checkIfGrounded();

        //checks if the player is touching a ladder
        checkIfClimbing();

        //climb if needed
        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.position.x, verticalMoveInput * ladderSpeed);
            rb.gravityScale = 0;
        }
        else
            rb.gravityScale = 2;

    }

    //frame
    void Update()
    {
        //spawns echo effect
        if(horizontalMoveInput!=0)
        {
            if (timeBtwEchoSpawns <= 0)
            {
                Destroy(Instantiate(echoEffect, transform.position, Quaternion.identity), 2f);
                timeBtwEchoSpawns = startTimeBtwEchoSpawns;
            }
            else
                timeBtwEchoSpawns -= Time.deltaTime;
        }


        //checks if need to flip sprite direction
        if (!facingRight && horizontalMoveInput > 0 || facingRight && horizontalMoveInput < 0)
            flip();


        if (isGrounded)
        {
            if (justLanded)
            {
                //transform.Translate(Vector2.right * speed * Time.deltaTime);
                Instantiate(dustpuff, transform);
                justLanded = false;
                extraJumps = extraJumpsValue;
                CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
                rb.velocity = Vector2.up * jumpForce;
        }
        else
        {
            justLanded = true;
            if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
                Instantiate(dustpuff, transform);
            }
        }


        //attacks
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("||");
                Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemy);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    Debug.Log(enemiesToHit[i]);
                    enemiesToHit[i].GetComponent<EnemyPatrol>().takeDamage(damage);
                    count++;
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
            timeBtwAttack -= Time.deltaTime;
    }

//flip direction
    void flip()
    {
        if (facingRight)
            transform.eulerAngles = new Vector3(0, -180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
        facingRight = !facingRight;
    }

//moves according to horizontal input
    void move()
    {
        rb.velocity = new Vector2(horizontalMoveInput * speed, rb.velocity.y);
    }

//checks if the player is touching the ground
    void checkIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

//checks if the player is touching a ladder
    private void checkIfClimbing()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderDistance, whatIsLadder);
        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                isClimbing = true;
        }
        else
            isClimbing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
