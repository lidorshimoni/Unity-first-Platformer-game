using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    public float lives;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
            UnityEngine.Object.Destroy(player);

    }

    public void hitPlayer(float damage)
    {
        lives -= damage;
    }

    public IEnumerator KnockBack(float duration, float power)
    {
        Vector3 direction = player.transform.position;
        float timer = 0;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            player.GetComponent<Rigidbody2D>().AddForce(new Vector3(direction.x * -100, direction.y * power, player.transform.position.z));
        }
        yield return null;
    }
}
