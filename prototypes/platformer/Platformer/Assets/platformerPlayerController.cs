using UnityEngine;
using UnityEngine.UI;

public class platformerPlayerController : MonoBehaviour
{
    private Animator animator;
    
    [SerializeField] private GameObject cam;
    private CharacterController cc;
    
    private Vector3 velocity = Vector3.zero;
    private float yVelocity;
    private bool isGrounded;
    
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float exhaustedSpeed = 2f; // Slow speed when exhausted
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float glideGravity = -2f;

    [SerializeField] private Image StaminaBar;
    [SerializeField] public float stamina, maxStamina;
    private bool isGliding = false;
    private bool isSprinting = false;
    private bool isExhausted = false;
    public float glideCost = 80f;
    public float sprintCost = 80f;
    public float staminaRegenRate = 40f;
    private int seedCount = 0;

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

        // Handle exhaustion state
        if (stamina <= 0)
        {
            isExhausted = true;
            isSprinting = false;
            isGliding = false;
        }
        else if (stamina >= maxStamina)
        {
            isExhausted = false;
        }

        isSprinting = isGrounded && Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isMoving && !isExhausted;
        isGliding = Input.GetKey(KeyCode.Space) && !isGrounded && yVelocity < 0 && stamina > 0 && !isExhausted;
        
        // Animation Logic
        if (isExhausted)
        {
            animator.SetBool("isExhausted", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", false);
        }
        else if (isGrounded)
        {
            animator.SetBool("isExhausted", false);
            animator.SetBool("isRunning", isSprinting);
            animator.SetBool("isWalking", !isSprinting && isMoving);
            animator.SetBool("isIdle", !isMoving);
        }

        // Movement Speed Control
        float currentSpeed = isExhausted ? exhaustedSpeed : (isSprinting ? sprintSpeed : moveSpeed);

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
            stamina -= sprintCost * Time.deltaTime;
        }
        if (isGliding)
        {
            yVelocity += (glideGravity - gravity) * Time.deltaTime;
            stamina -= glideCost * Time.deltaTime;
        }
        else if (!isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        if (isGrounded)
        {
            yVelocity = -0.5f;
            if (!isSprinting && !isGliding && stamina < maxStamina)
            {
                stamina += staminaRegenRate * Time.deltaTime;
            }
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        StaminaBar.fillAmount = stamina / maxStamina;

        velocity.y = yVelocity;
        velocity = Vector3.ClampMagnitude(velocity, 10f);
        cc.Move(velocity * Time.deltaTime);

        if (velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z)), 10f * Time.deltaTime);
        }
    }

    public void CollectSeed()
    {
        seedCount++;
        glideCost = Mathf.Max(10f, 80f - (seedCount * 5));
        sprintCost = Mathf.Max(10f, 80f - (seedCount * 5));
        Debug.Log("Seed Collected! Seeds: " + seedCount);
        Debug.Log("New Glide Cost: " + glideCost);
        Debug.Log("New Sprint Cost: " + sprintCost);
    }
}
