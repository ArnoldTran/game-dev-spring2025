using UnityEngine;
using UnityEngine.UI;

public class platformerPlayerController : MonoBehaviour
{
    // Animation
    private Animator animator;

    // Character Movement
    [SerializeField] private GameObject cam;
    private CharacterController cc;

    private Vector3 velocity = Vector3.zero;
    private float yVelocity;
    private bool isGrounded, wasSprinting;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintSpeed = 10f; // Sprint speed
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float glideGravity = -2f;

    // Stamina
    [SerializeField] private UnityEngine.UI.Image StaminaBar;
    [SerializeField] public float stamina, maxStamina;
    private bool isGliding = false;
    private bool isSprinting = false;
    public float glideCost = 20f;
    public float sprintCost = 10f; // Stamina cost per second when sprinting
    public float staminaRegenRate = 40f; // Regeneration rate

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = cc.isGrounded; // Check if Player is on ground
        
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        
        bool isMoving = hAxis != 0 || vAxis != 0;
        isSprinting = isGrounded && Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isMoving;
        // Animation States
        if (isGrounded)
        {
            if (isSprinting)
                {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                }
            else if (isMoving)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
            }

        Debug.Log("Idle: " + animator.GetBool("isIdle"));
        Debug.Log("Walking: " + animator.GetBool("isWalking"));
        Debug.Log("Running: " + animator.GetBool("isRunning"));
    }

        // Sprint only if on the ground and stamina > 0
        if (isGrounded)
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0 && (hAxis != 0 || vAxis != 0);
            wasSprinting = isSprinting; // Track if sprinting before jumping
        }
        else
        {
            isSprinting = false; // Disable sprinting mid-air
        }

        // Glide only if stamina is greater than 0
        isGliding = Input.GetKey(KeyCode.Space) && !isGrounded && yVelocity < 0 && stamina > 0;

        if (isGrounded)
        {
            yVelocity = -0.5f;
            // Regenerate stamina when grounded and not sprinting or gliding
            if (!isSprinting && !isGliding)
            {
                stamina += staminaRegenRate * Time.deltaTime;
            }
        }

        // Movement speed
        float currentSpeed = wasSprinting ? sprintSpeed : moveSpeed; // Keep sprint speed if airborne after sprinting

        velocity = new Vector3(0, yVelocity, 0);

        Vector3 adjustedCamRight = cam.transform.right;
        adjustedCamRight.y = 0;
        adjustedCamRight.Normalize();
        velocity += adjustedCamRight * hAxis * currentSpeed;

        Vector3 adjustedCamForward = cam.transform.forward;
        adjustedCamForward.y = 0;
        adjustedCamForward.Normalize();
        velocity += adjustedCamForward * vAxis * currentSpeed;

        // Sprint stamina drain
        if (isSprinting)
        {
            stamina -= sprintCost * Time.deltaTime;
        }

        // Jump
        if (cc.isGrounded)
        {
            yVelocity = -0.5f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jumpForce;
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        // Glide (Slow Fall)
        if (isGliding)
        {
            yVelocity += (glideGravity - gravity) * Time.deltaTime;
            stamina -= glideCost * Time.deltaTime;
        }
        else if (!isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        // Ensure stamina is always between 0 and maxStamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        StaminaBar.fillAmount = stamina / maxStamina;

        velocity.y = yVelocity;
        velocity = Vector3.ClampMagnitude(velocity, 10f);

        cc.Move(velocity * Time.deltaTime);

        // Duck Rotation
        Vector3 moveDirection = new Vector3(velocity.x, 0, velocity.z);
        if (moveDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
        }
    }
}
