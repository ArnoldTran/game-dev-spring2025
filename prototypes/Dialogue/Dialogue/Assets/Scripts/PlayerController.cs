using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust speed here
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from player (WASD or Arrow Keys)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // Prevent diagonal movement from being faster
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.linearVelocity = moveInput * moveSpeed;
    }
}


