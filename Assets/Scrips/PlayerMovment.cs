using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
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

    private int extraJumps;
    public int extraJumpsValue;

    private SpriteRenderer sr;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        horizontalMoveInput = Input.GetAxis("Horizontal");
        verticalMoveInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontalMoveInput * speed, rb.velocity.y);
        if (!facingRight && horizontalMoveInput > 0)
            flip();
        else if (facingRight && horizontalMoveInput < 0)
            flip();

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderDistance, whatIsLadder);
        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
                //Debug.Log("<color=red>climbing</color>");
            }
        }
        else
        {
            isClimbing = false;
            //Debug.Log("<color=blue>not</color>");
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.position.x, verticalMoveInput * ladderSpeed);
            rb.gravityScale = 0;
        }
        else
            rb.gravityScale = 2;

    }


    void Update()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                rb.velocity = Vector2.up * jumpForce;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
            }
        }
    }
    void flip()
    {
        sr.flipX = !sr.flipX;
        facingRight = !facingRight;
    }
}
