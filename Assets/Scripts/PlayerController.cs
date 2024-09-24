using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("A reference to the player's Rigidbody2D component.")]
    public Rigidbody2D rigidbody;
    
    [Tooltip("The fastest speed the player can move left/right, in units per second.")]
    public float topSpeed = 3;
    [Tooltip("The speed at which the player accelerates to the left/right, in units per second.")]
    public float accelerationRate = 6;
    [Tooltip("The speed at which the player decelerates to the left/right, in units per second.")]
    public float decelerationRate = 6;

    public GameOver GameOver;

    SpriteRenderer spriteRenderer;
    bool isWrappingX = false;

    public bool enemyHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    bool CheckRenderers()
    {
        if(spriteRenderer.isVisible)
        {
            return true;
        }
        // Otherwise, the object is invisible 
        return false;
    }

    void ScreenWrap()
    {
        var isVisible = CheckRenderers();
        if(isVisible)
        {
            isWrappingX = false;
            return;
        }
        if(isWrappingX) {
            return;
        }
        
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;
        
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ScreenWrap(); // Screenwrap on left/right sides of the screen

        Vector2 velocity = rigidbody.velocity;

        bool isMovingLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isMovingRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        
        if (isMovingLeft)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, -topSpeed, accelerationRate * Time.deltaTime);  
            spriteRenderer.flipX = true;        
        }
        if (isMovingRight)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, topSpeed, accelerationRate * Time.deltaTime);
            spriteRenderer.flipX = false;
        }

        if (!isMovingLeft && !isMovingRight)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, decelerationRate * Time.deltaTime);
        }

        rigidbody.velocity = velocity;
    }
    
    public void Lose()
    {
        enabled = false;

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < 0)
        {
            GameOver.Setup();
        }
    }
}
