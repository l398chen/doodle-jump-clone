using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("A reference to the player to follow.")]
    public PlayerController player;

    [Tooltip("The relative distance the player must be from the camera before the camera starts moving up.")]
    public float topBorder = 3;
    [Tooltip("The relative distance the player must be from the camera before the camera starts moving down.")]
    public float bottomBorder = -5;

    public bool moveUp = true;
    public bool moveDown = false;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        Vector3 playerPosition = player.transform.position;

        if (moveUp && playerPosition.y > position.y + topBorder)
        {
            position.y = playerPosition.y - topBorder;
        }
        if (playerPosition.y < position.y + bottomBorder)
        {
            if (moveDown)
            {
                position.y = playerPosition.y - bottomBorder;
            }
            else
            {
                player.Lose();
            }
        }
        
        transform.position = position;
    }

    // This is a special function that lets you draw debug lines on the screen. It's really handy!
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        const float lineWidth = 10f;
        Gizmos.DrawLine(transform.position + Vector3.up * topBorder + Vector3.left * lineWidth / 2,
            transform.position + Vector3.up * topBorder + Vector3.right * lineWidth / 2);
        Gizmos.DrawLine(transform.position + Vector3.up * bottomBorder + Vector3.left * lineWidth / 2,
            transform.position + Vector3.up * bottomBorder + Vector3.right * lineWidth / 2);
    }
}