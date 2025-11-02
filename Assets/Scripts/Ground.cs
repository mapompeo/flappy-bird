using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        var bird = other.gameObject.GetComponent<Bird>();
        
        if(bird == null) return;
        
        bird.GameOver();
    }
}
