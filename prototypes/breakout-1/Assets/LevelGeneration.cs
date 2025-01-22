using System.Numerics;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
public Vector2Int size;
public UnityEngine.Vector2 offset;
public GameObject Brick;

    private void Awake()
    {
        for (int i = 0; i < size.x; i++)
    {
        for (int j = 0; j < size.y; j++)
        {
            // Instantiate a new brick
            GameObject newBrick = Instantiate(Brick, transform);

            // Calculate the position of the brick in the grid
            float xPos = (i - (size.x - 1) * 0.5f) * offset.x;
            float yPos = (j - (size.y - 1) * 0.5f) * offset.y;

            newBrick.transform.position = transform.position + new UnityEngine.Vector3(xPos, yPos, 0);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
