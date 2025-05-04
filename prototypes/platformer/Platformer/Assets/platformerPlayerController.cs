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
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float glideGravity = -2f;

    // Stamina
    [SerializeField] private Image StaminaBar;
    [SerializeField] public float stamina, maxStamina;
    private bool isGliding = false;
    private bool isSprinting = false;
    private bool isExhausted = false; // 👈 Added
    public float glideCost = 20f;
    public float sprintCost = 10f;
    public float staminaRegenRate = 40f;

    // Seed Tracking
    public int seedCount = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isGrounded = cc.isGrounded;

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        bool isMoving = hAxis != 0 || vAxis != 0;

        // Update exhaustion state
        if (stamina <= 0)
        {
            isExhausted = true; // 👈 Now exhausted
        }
        else if (stamina >= maxStamina)
        {
            isExhausted = false; // 👈 Recovered
        }

        // Update costs with seeds
        float reducedGlideCost = Mathf.Max(0, glideCost - (5 * seedCount));
        float reducedSprintCost = Mathf.Max(0, sprintCost - (5 * seedCount));

        // Animation
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
        }

        // Sprint & Glide Restrictions
        isSprinting = isGrounded && Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isMoving && !isExhausted; // 👈
        isGliding = Input.GetKey(KeyCode.Space) && !isGrounded && yVelocity < 0 && stamina > 0 && !isExhausted; // 👈

        if (isGrounded)
        {
            yVelocity = -0.5f;
            if (!isSprinting && !isGliding)
            {
                stamina += staminaRegenRate * Time.deltaTime;
            }
        }

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        velocity = new Vector3(0, yVelocity, 0);

        Vector3 adjustedCamRight = cam.transform.right;
        adjustedCamRight.y = 0;
        adjustedCamRight.Normalize();
        velocity += adjustedCamRight * hAxis * currentSpeed;

        Vector3 adjustedCamForward = cam.transform.forward;
        adjustedCamForward.y = 0;
        adjustedCamForward.Normalize();
        velocity += adjustedCamForward * vAxis * currentSpeed;

        if (isSprinting)
        {
            stamina -= reducedSprintCost * Time.deltaTime;
        }

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

        if (isGliding)
        {
            yVelocity += (glideGravity - gravity) * Time.deltaTime;
            stamina -= reducedGlideCost * Time.deltaTime;
        }
        else if (!isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        StaminaBar.fillAmount = stamina / maxStamina;

        velocity.y = yVelocity;
        velocity = Vector3.ClampMagnitude(velocity, 10f);

        cc.Move(velocity * Time.deltaTime);

        Vector3 moveDirection = new Vector3(velocity.x, 0, velocity.z);
        if (moveDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
        }
    }

    public void CollectSeed()
    {
        seedCount++;
        Debug.Log("Seeds Collected: " + seedCount);
    }
}
