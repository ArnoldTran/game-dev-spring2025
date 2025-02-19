using System.Numerics;
using UnityEngine;

public class platformerPlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject cam;
    CharacterController cc;
    UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero;

    float moveSpeed = 12f;

    float yVelocity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        velocity = UnityEngine.Vector3.zero;


        UnityEngine.Vector3 adjustedCamRight = cam.transform.right; // Horizontal Movement
        adjustedCamRight.y = 0;
        adjustedCamRight.Normalize();
        velocity += adjustedCamRight *hAxis * moveSpeed;

        UnityEngine.Vector3 adjustedCamForward = cam.transform.forward; // Vertical Movement
        adjustedCamForward.y = 0;
        adjustedCamForward.Normalize();
        velocity += adjustedCamForward *vAxis * moveSpeed;

        if (!cc.isGrounded) {
            yVelocity += -9.81f * Time.deltaTime; // Gravity
        } else {
            yVelocity = -0.1f;
            Debug.Log("I'm touching the ground");

            if (Input.GetKeyDown(KeyCode.Space)) { // If hit SpaceBar
            yVelocity = 4f; // Jump Velocity
            }
        }

        velocity = UnityEngine.Vector3.ClampMagnitude(velocity, 10);
        cc.Move(velocity * Time.deltaTime); //Update Position
    }
}
