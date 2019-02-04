using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float lives;
    public float EnemySpeed;
    public float RayCastDistance;
    public float damage;

    public float knockBackForce;
    public float knockBackDuration;


    public bool movingRight = true;
    public Transform groundDetection;
    public PlayerManeger pm;
    public LayerMask whatIsGround;
    public GameObject BloodEffect;

    void Update()
    {
        if (lives <= 0)
            UnityEngine.Object.Destroy(this.gameObject);
        transform.Translate(Vector2.right * EnemySpeed * Time.deltaTime);

        //RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, RayCastDistance, whatIsGround);
        //if(!groundInfo.collider || !groundInfo.collider.CompareTag("Ground"))

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, RayCastDistance, whatIsGround);
        if(!groundInfo.collider)
        {
            if(movingRight)
                transform.eulerAngles = new Vector3(0, -180, 0);
            
            else
                transform.eulerAngles = new Vector3(0, 0, 0);

            movingRight = !movingRight;
        }
       
    }

    //if enemy touched player
    //void OnCollisionEnter2D(Collision2D colider)
    //{
    //    if (colider.collider.CompareTag("Player"))
    //    {
    //        pm.hitPlayer(damage);
    //        StartCoroutine(pm.KnockBack(knockBackDuration, knockBackForce));
    //    }
    //}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            pm.hitPlayer(damage);
            Debug.Log("Col");
            Rigidbody2D rig = col.gameObject.GetComponent<Rigidbody2D>();
            if (rig == null) {  Debug.Log("shit");return; }
            rig.AddForce(-rig.velocity * knockBackForce);
        }
    }

    public void takeDamage(float damage)
    {
        this.lives -= damage;
        Instantiate(BloodEffect, transform);
    }
}
