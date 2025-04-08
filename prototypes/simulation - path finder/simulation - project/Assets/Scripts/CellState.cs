// using UnityEngine;

public class CellState
{
    public int x;
    public int y;
    public enum terainType { GRASS, ROAD, HOUSE, DIRT };
    public terainType cellTerrain;
    public float height;

    public string pathStateVisuals = "default";

    public CellState Clone()
    {
        return new CellState
        {
            x = this.x,
            y = this.y,
            height = this.height,
            cellTerrain = this.cellTerrain,
            pathStateVisuals = this.pathStateVisuals
        };
    }
}
