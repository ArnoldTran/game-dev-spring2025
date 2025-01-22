using UnityEngine;
using TMPro;
public class BallScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    public float minY = -4f;
    public float maxVelocity = 15f;
    public float speedIncreaseFactor = 1.1f; // Factor to increase speed
    int score = 0;
    int lives = 5;

    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector3(3, -5, 0); // Use `velocity` instead of `linearVelocity`
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY)
        {
            if (lives <= 0)
            {
                GameOver();
            }
            else
            {
                // Reset ball position and velocity
                transform.position = new Vector3(0, 3, 0);
                rb.linearVelocity = new Vector3(3, -5, 0);
                lives--;
                livesImage[lives].SetActive(false);
            }
        }

        // Clamp velocity to the maximum allowed value
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("brick"))
        {
            Destroy(collision.gameObject);
            score += 10;
            scoreTxt.text = score.ToString("0000");

            // Increase the ball's velocity
            rb.linearVelocity *= speedIncreaseFactor;

            // Clamp the velocity to the maximum allowed value
            if (rb.linearVelocity.magnitude > maxVelocity)
            {
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }
}
