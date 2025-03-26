using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;  // The prefab for the hex tile (must be set in the Inspector)
    public float hexSize = 1f;        // Size of each hex tile (radius from center to a flat side)
    public float gapSize = 0.1f;      // Size of the gap between the hex tiles
    public int gridWidth = 10;        // Number of hex tiles horizontally
    public int gridHeight = 10;       // Number of hex tiles vertically
    public Vector2 gridOffset = new Vector2(0, 0); // Offset to position the grid in the scene

    private List<GameObject> hexTiles = new List<GameObject>();  // List to hold all the hex tiles

    void Start()
    {
        GenerateGrid();  // Generate the hex grid when the scene starts
    }

    void GenerateGrid()
    {
        float width = Mathf.Sqrt(3) * hexSize + gapSize;  // Width between the centers of two adjacent hexagons (including gap)
        float height = 2 * hexSize + gapSize;  // Height between rows of hexagons (including gap)

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate the position for each hex tile
                float xOffset = x * width;
                float zOffset = y * height;

                // Stagger the rows (odd rows are offset)
                if (y % 2 != 0)
                {
                    xOffset += width / 2;  // Offset every other row
                }

                // Apply the grid offset
                Vector3 position = new Vector3(xOffset + gridOffset.x, 0, zOffset + gridOffset.y);

                // Instantiate the hex tile at the calculated position
                GameObject hexTile = Instantiate(hexTilePrefab, position, Quaternion.identity);
                hexTile.name = $"HexTile_{x}_{y}";  // Give each tile a unique name

                // Access the HexTile component and initialize its statistics
                HexTile hexTileScript = hexTile.GetComponent<HexTile>();

                // Set the statistics or other parameters here if necessary
                // Example: Setting specific values to tile properties if needed
                // hexTileScript.temperature = ...;

                hexTiles.Add(hexTile);  // Add to the list of tiles
                hexTile.transform.parent = transform;  // Set the parent to this GameObject (optional)
            }
        }
    }
}
