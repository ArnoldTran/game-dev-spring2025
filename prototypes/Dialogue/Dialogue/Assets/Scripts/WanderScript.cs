using UnityEngine;

public class NPCRandomWander : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float walkTime = 2f;
    public float stopDistance = 2f;
    public Transform player;

    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -3.5f;
    public float maxY = 5.5f;

    private float walkCounter;
    private int walkDirection;
    private Vector2 moveDirection;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Rigidbody2D rb;

    // Reference to the Spacebar image prefab
    public GameObject interactImagePrefab;  // Drag the Spacebar prefab here in the inspector
    private GameObject interactImageInstance;  // This will store the instantiated Spacebar image

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkCounter = walkTime;
        ChooseNewDirection();
    }

    void Update()
    {
        // Stop moving if near the player
        if (player != null && Vector2.Distance(transform.position, player.position) < stopDistance)
        {
            // Stop movement and set velocity to zero
            rb.linearVelocity = Vector2.zero;

            // If the spacebar image isn't instantiated yet, instantiate it and position it above the NPC
            if (interactImageInstance == null && interactImagePrefab != null)
            {
                interactImageInstance = Instantiate(interactImagePrefab, transform.position, Quaternion.identity);
                interactImageInstance.transform.SetParent(transform);  // Make it a child of the NPC
                interactImageInstance.GetComponent<RectTransform>().localPosition = new Vector3(0f, 1.5f, 0f);  // Position above NPC
            }

            // Spacebar interaction logic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Interacted with NPC!");
                // trigger dialogue or event here
            }

            return; // Prevent further wandering logic if close to player
        }
        else
        {
            // If the player is not near, destroy the spacebar image
            if (interactImageInstance != null)
            {
                Destroy(interactImageInstance);
            }
        }

        // Continue wandering logic when not near the player
        walkCounter -= Time.deltaTime;

        // Keep moving in the same direction
        rb.linearVelocity = moveDirection * moveSpeed;

        // Check bounds
        Vector2 nextPos = rb.position + rb.linearVelocity * Time.deltaTime;
        if (nextPos.x < minX || nextPos.x > maxX || nextPos.y < minY || nextPos.y > maxY)
        {
            ChooseNewDirection();
        }

        // Change direction if walkCounter hits zero
        if (walkCounter <= 0)
        {
            walkCounter = walkTime;
            ChooseNewDirection();
        }
    }

    void ChooseNewDirection()
    {
        walkDirection = Random.Range(0, directions.Length);
        moveDirection = directions[walkDirection];
    }
}
