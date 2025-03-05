using UnityEngine;

public class SeedCollectable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the seed is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with seed!"); // Debug if the player collides with the seed
            platformerPlayerController playerController = other.GetComponent<platformerPlayerController>();
            if (playerController != null)
            {
                playerController.CollectSeed(); // Call method to increase seed count
                Destroy(gameObject); // Destroy seed object after collection
            }
            else
            {
                Debug.LogWarning("Player script not found on: " + other.name);
            }
        }
        else
        {
            Debug.Log("Non-player collided with seed: " + other.name); // Debug for non-player collision
        }
    }
}
