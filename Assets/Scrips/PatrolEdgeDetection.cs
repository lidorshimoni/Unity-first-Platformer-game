using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEdgeDetection : MonoBehaviour
{
    public float speed;

    public float distance;

    public bool movingRight = true;
    public Transform groundDetection;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if(!groundInfo.collider)
        {
            Debug.Log("something");
            if(movingRight)
                transform.eulerAngles = new Vector3(0, -180, 0);
            
            else
                transform.eulerAngles = new Vector3(0, 0, 0);

            movingRight = !movingRight;
        }
    }
}
