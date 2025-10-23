using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyChãoItj : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        var bird = other.gameObject.GetComponent<FlappyBirdPlayerItj>();
        
        if(bird == null) return;
        
        bird.GameOver();
    }
}
