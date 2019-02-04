using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatformOverTime : MonoBehaviour
{
    private bool playerOnBoard;
    public float destroyDelay;

    void OnCollisionEnter2D()
    {
        Destroy(gameObject, destroyDelay);
    }
}
