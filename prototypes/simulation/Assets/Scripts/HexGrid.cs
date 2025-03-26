using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float hexSize;
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private HexOrientation orientation;  // Serialized enum for inspector

    public int Width => width;
    public int Height => height;
    public float HexSize => hexSize;
    public GameObject HexPrefab => hexPrefab;
    public HexOrientation Orientation => orientation;  // Getter for the orientation

    private Vector3[] corners;  // Cache corners for performance

    private void OnDrawGizmos()
    {
        // Cache the corners once, depending on orientation and hex size
        corners = HexMetrics.Corners(hexSize, orientation);

        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, z, orientation) + transform.position;

                // Draw hexagon using the cached corners
                for (int s = 0; s < corners.Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + corners[s],
                        centrePosition + corners[(s + 1) % corners.Length]  // Wrap around at the end
                    );
                }
            }
        }
    }
}

public enum HexOrientation
{
    FlatTop,   // Hexagons with flat sides on top and bottom
    PointyTop  // Hexagons with pointy sides on top and bottom
}
