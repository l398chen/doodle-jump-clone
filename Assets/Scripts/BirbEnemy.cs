using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

public class BirbEnemy : MonoBehaviour
{
    public AudioClip enemySound;
    public float enemySoundVolume = 1;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private int moveSpeed;

    bool atEdge = false;

    bool CheckRenderers()
    {
        if(spriteRenderer.isVisible)
        {
            return true;
        }
        // Otherwise, the object is invisible 
        return false;
    }

    void PatrolMovement()
    {
        var isVisible = CheckRenderers();
        if(isVisible)
        {
            atEdge = false;
            return;
        }
        if(atEdge) {
            return;
        }

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        
        if (viewportPosition.x > 1 || viewportPosition.x < 0)
        {
            atEdge = true;
            rigidbody.velocity = Vector2.zero;

            spriteRenderer.flipX = moveSpeed > 0 ? true : false;

            moveSpeed = -moveSpeed;
            rigidbody.velocity = new Vector2(moveSpeed, 0);            
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;

        moveSpeed = Random.Range(1, 10);
        
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(moveSpeed, 0);
    }

    void Update()
    {
        PatrolMovement();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && other.TryGetComponent(out Rigidbody2D rigidbody))
        {
            if (enemySound != null)
            {
                // This function creates a temporary new game object with a AudioSource attached. We need to do this so
                // that the sound effect continues to play if the platform is destroyed.
                AudioSource.PlayClipAtPoint(enemySound, transform.position, enemySoundVolume);
            }

            playerController.enemyHit = true;

            Rigidbody2D playerRB = playerController.rigidbody;

            playerRB.gravityScale = 3f; // Jumping upward gravity scale
                
            Vector2 velocity = playerRB.velocity;

            double x_dir = 0;
            double y_dir = -5;

            Vector2 force = new Vector2((float)x_dir, (float)y_dir);

            rigidbody.AddForce(force, ForceMode2D.Impulse); // Apply force to player
        }
    }
}
