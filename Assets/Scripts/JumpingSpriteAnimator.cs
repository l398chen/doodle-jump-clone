using UnityEngine;

public class JumpingSpriteAnimator : MonoBehaviour
{
    public Sprite jumpingSprite;
    public Sprite fallingSprite;

    [Tooltip("If the sprite is rendered facing left, uncheck this.")]
    public bool isFacingRight = true;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;

    public void Start()
    {
        // We're doing this procedurally so you don't have to manually assign the sprite renderer in the Unity inspector
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        // Set the sprite to be either jumping or falling, based on the player's speed
        spriteRenderer.sprite = rigidbody.velocity.y > 0 ? jumpingSprite : fallingSprite;

        // Flip the sprite depending on the direction we are moving
        if (rigidbody.velocity.x > 0)
        {
            spriteRenderer.flipX = !isFacingRight;
        }
        else if (rigidbody.velocity.x < 0)
        {
            spriteRenderer.flipX = isFacingRight;
        }
    }
}