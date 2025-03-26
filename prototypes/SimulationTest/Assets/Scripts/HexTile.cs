using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexTile : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private MeshRenderer meshRenderer;
    private static readonly float OuterRadius = 1f;
    private static readonly float InnerRadius = OuterRadius * 0.866f; // sin(60°) for inner radius
    private Color defaultColor;
    private Color currentTileColor; // To hold the original color of the tile

    // Tile statistics
    public float temperature; // Temperature in °C
    public float precipitation; // Precipitation in mm/year
    public float humidity; // Humidity in percentage
    public float altitude; // Altitude in meters
    public float soilFertility; // Scale from 0 to 1 for soil fertility

    public enum TileType { Desert, Plain, Water }
    private TileType tileType;

    private void Awake()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateHexMesh();

        // Add a MeshCollider for interaction
        gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;

        // Save the default color of the material
        defaultColor = meshRenderer.material.color;
        currentTileColor = defaultColor; // Set the initial color as default color

        // Initialize statistics and determine tile type
        InitializeTileStatistics();
        DetermineTileType();
    }

    private void GenerateHexMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Define the six vertices of the hexagon
        vertices = new Vector3[] 
        {
            new Vector3(0, 0, OuterRadius),
            new Vector3(InnerRadius, 0, 0.5f * OuterRadius),
            new Vector3(InnerRadius, 0, -0.5f * OuterRadius),
            new Vector3(0, 0, -OuterRadius),
            new Vector3(-InnerRadius, 0, -0.5f * OuterRadius),
            new Vector3(-InnerRadius, 0, 0.5f * OuterRadius),
            new Vector3(0, 0, 0) // Center point
        };

        // Define triangles (each hexagon is made of six triangles)
        triangles = new int[]
        {
            6, 0, 1,
            6, 1, 2,
            6, 2, 3,
            6, 3, 4,
            6, 4, 5,
            6, 5, 0
        };

        // Assign mesh vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Optionally, set UVs for texturing (you can add your own textures here)
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / OuterRadius, vertices[i].z / OuterRadius); // Normalize UVs
        }
        mesh.uv = uvs;
    }

    // Set the color of the tile (used during grid generation)
    public void SetTileColor(Color color)
    {
        currentTileColor = color;
        meshRenderer.material.color = currentTileColor; // Apply the tile's color
    }

    // Initialize the tile's statistics (randomized for now)
private void InitializeTileStatistics()
{
    // Generate random values
    temperature = Random.Range(0f, 40f); // More likely to generate desert conditions
    precipitation = Random.Range(0f, 2000f); // More chance for water tiles
    humidity = Random.Range(10f, 80f);

    // Print stats for debugging
    Debug.Log($"Tile Stats: Temp={temperature}, Precip={precipitation}, Humidity={humidity}, Altitude={altitude}, SoilFertility={soilFertility}");

    DetermineTileType();
}

private void DetermineTileType()
{

    if (temperature > 25f && precipitation < 500f && humidity < 40f)
    {
        tileType = TileType.Desert;
        SetTileColor(Color.yellow); // Desert color
    }
    // Easier to form water tiles
    else if (precipitation > 1000f && humidity > 50f)
    {
        tileType = TileType.Water;
        SetTileColor(Color.blue); // Water color
    }
    // Default to Plain
    else
    {
        tileType = TileType.Plain;
        SetTileColor(Color.green);
    }

    Debug.Log($"Tile Type Assigned: {tileType}");
}


    // Accessor methods for tile statistics
    public TileType GetTileType()
    {
        return tileType;
    }

    private void OnMouseEnter()
{
    if (meshRenderer != null)
    {
        meshRenderer.material.color = Color.black; // Highlight on hover
    }

    // Print the tile's statistics to the console
    Debug.Log($"Hovered Tile Stats:\n" +
              $"Temperature: {temperature}°C\n" +
              $"Precipitation: {precipitation} mm/year\n" +
              $"Humidity: {humidity}%\n" +
              $"Tile Type: {tileType}");
}

private void OnMouseExit()
{
    if (meshRenderer != null)
    {
        meshRenderer.material.color = currentTileColor; // Reset to the tile's original color
    }
}
}
