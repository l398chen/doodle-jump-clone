﻿using System;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [Tooltip("The velocity at which the player will jump when they touch this platform.")]
    public float jumpVelocity = 5f;
    [Tooltip("If this platform is temporary, it will be destroyed when the player jumps off it.")]
    public bool temporary = false;
    [Tooltip("If true, this platform will give the player a jump boost even if they are approaching from below.")]
    public bool allowCollisionsFromBelow = false;
    [Tooltip("The amount of score the player gets if they jump on this. This should only be used on temporary platforms, otherwise the player can get infinite score easily.")]
    public float scoreOnJump = 0;

    public AudioClip jumpSound;
    public float jumpSoundVolume = 1;

    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && other.TryGetComponent(out Rigidbody2D rigidbody))
        {
            if (playerController.enemyHit)
            {
                return;
            }
            
            if (!playerController.enabled)
            {
                return;
            }
            
            if (!allowCollisionsFromBelow && rigidbody.velocity.y > 0)
            {
                return;
            }

            if (jumpVelocity != 0)
            {
                rigidbody.gravityScale = 1; // Jumping upward gravity scale
                
                Vector2 velocity = rigidbody.velocity;
                
                float rotation = gameObject.transform.rotation.eulerAngles[2]; // Get z parameter of the rotation quaternion in degrees
                // Debug.Log("Rotation: " + rotation); //Print rotation value to the console

                double x_dir = -jumpVelocity*Math.Sin(rotation*Math.PI/180);
                double y_dir = jumpVelocity*Math.Cos(rotation*Math.PI/180);

                Vector2 force = new Vector2((float)x_dir, (float)y_dir);

                if (velocity.y < 0) //checks whether player is falling
                    force.y -= velocity.y;
                    rigidbody.gravityScale = 1.5f; // Change to falling gravity scale

                rigidbody.AddForce(force, ForceMode2D.Impulse); // Apply force to player
            }
            
            if (scoreOnJump != 0 && Score.Instance != null)
            {
                Score.Instance.AddScore(scoreOnJump);
            }

            if (jumpSound != null)
            {
                // This function creates a temporary new game object with a AudioSource attached. We need to do this so
                // that the sound effect continues to play if the platform is destroyed.
                AudioSource.PlayClipAtPoint(jumpSound, transform.position, jumpSoundVolume);
            }
            
            if (temporary)
            {
                Destroy(gameObject);
            }
        }
    }
}