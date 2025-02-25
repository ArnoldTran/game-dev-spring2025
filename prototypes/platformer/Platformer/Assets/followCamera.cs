using UnityEngine;

public class MouseCameraController : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float mouseSensitivity = 2f; // Adjust sensitivity as needed
    public float distanceFromPlayer = 10f; // Distance between the camera and the player

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust yaw (horizontal rotation)
        yaw += mouseX;

        // Adjust pitch (vertical rotation) and clamp it to avoid flipping
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -40f, 80f); // Adjust as needed

        // Calculate new camera position
        Vector3 offset = new Vector3(0, 2f, -distanceFromPlayer); // Offset to position the camera behind the player
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = player.position + rotation * offset;
        
        // Make camera look at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust as needed
    }
}


