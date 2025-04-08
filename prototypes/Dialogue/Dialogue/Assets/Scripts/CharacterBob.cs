using UnityEngine;

public class CharacterBob : MonoBehaviour
{
    public Transform body;         // Reference to the body part of NPC/Character
    public Transform face;         // Reference to the face part of NPC/Character

    public float bobFrequency = 10f;   // Speed of bobbing (how fast it moves)
    public float bobAmplitude = 0.05f; // How far it moves up/down or left/right
    public float bobSpeedThreshold = 0.1f; // Minimum speed to start bobbing
    public float hopHeight = 0.1f; // How high the character hops up/down
    public float rotationAmount = 5f; // How much the character rotates (in degrees)

    private Vector3 bodyStartPos;
    private Vector3 faceStartPos;
    private Rigidbody2D rb;

    private bool isFacingRight = true;  // Track the current facing direction

    void Start()
    {
        // Get Rigidbody2D if this NPC is meant to move
        rb = GetComponent<Rigidbody2D>();
        
        // Initialize starting positions for body and face
        bodyStartPos = body.localPosition;
        faceStartPos = face.localPosition;
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;  // Speed of movement
        float moveDirection = rb.linearVelocity.x;  // X-axis velocity (left or right movement)

        if (speed > bobSpeedThreshold)
        {
            // Horizontal Bobbing (left and right)
            float offsetX = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            body.localPosition = bodyStartPos + new Vector3(offsetX, 0f, 0f);
            face.localPosition = faceStartPos + new Vector3(offsetX, 0f, 0f);  // Optional: opposite direction for face

            // Vertical Bobbing (Hop)
            float offsetY = Mathf.Sin(Time.time * bobFrequency * 2) * hopHeight;  // Slight vertical bounce
            body.localPosition = new Vector3(body.localPosition.x, bodyStartPos.y + offsetY, body.localPosition.z);
            face.localPosition = new Vector3(face.localPosition.x, faceStartPos.y + offsetY, face.localPosition.z);

            // Rotation Bobbing (tilt)
            float angle = Mathf.Sin(Time.time * bobFrequency) * rotationAmount;
            body.localRotation = Quaternion.Euler(0f, 0f, angle);  // Rotate around Z-axis
            face.localRotation = Quaternion.Euler(0f, 0f, angle);  // Optional: rotate in opposite direction for face

            // Flip sprite for left/right movement
            if (moveDirection > 0) // Moving right
            {
                if (!isFacingRight)  // Only flip if not already facing right
                {
                    isFacingRight = true;
                    body.localScale = new Vector3(1f, 1f, 1f);
                    face.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else if (moveDirection < 0) // Moving left
            {
                if (isFacingRight)  // Only flip if not already facing left
                {
                    isFacingRight = false;
                    body.localScale = new Vector3(-1f, 1f, 1f);
                    face.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }
        else
        {
            // Reset to original position if idle, but keep the last flip direction
            body.localPosition = bodyStartPos;
            face.localPosition = faceStartPos;
            body.localRotation = Quaternion.identity;
            face.localRotation = Quaternion.identity;
        }
    }
}
