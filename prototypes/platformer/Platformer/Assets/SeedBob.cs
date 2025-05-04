using UnityEngine;

public class SeedBob : MonoBehaviour
{
    public float bobSpeed = 2f;     // Speed of the bobbing motion
    public float bobHeight = 0.25f; // Height of the bobbing motion
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}

