using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius(float hexSize)
    {
        return hexSize * 0.866025404f;
    }

    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }
        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index;

        if (orientation == HexOrientation.PointyTop)
        {
            angle += 30f; // Pointy-top hexagons rotate by 30 degrees
        }
        else
        {
            angle += 0f;  // Flat-top hexagons remain at 0 degrees
        }

        return new Vector3(
            hexSize * Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            hexSize * Mathf.Sin(angle * Mathf.Deg2Rad)
        );
    }

    public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return new Vector3(
                (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius(hexSize) * 2f),
                0f,
                z * (HexMetrics.OuterRadius(hexSize) * 1.5f)
            );
        }
        else
        {
            return new Vector3(
                x * (HexMetrics.OuterRadius(hexSize) * 1.5f),
                0f,
                (z + x * 0.5f - x / 2) * (HexMetrics.InnerRadius(hexSize) * 2f)
            );
        }
    }

    public static Vector3Int OffsetToCube(int col, int row, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return AxialToCube(OffsetToAxialPointy(col, row));
        }
        else
        {
            return AxialToCube(OffsetToAxialFlat(col, row));
        }
    }

    public static Vector3Int AxialToCube(Vector2Int axial)
    {
        int x = axial.x;
        int z = axial.y;
        int y = -x - z;
        return new Vector3Int(x, z, y);
    }

    public static Vector2Int OffsetToAxialFlat(int col, int row)
    {
        int q = col;
        int r = row - (col + (col & 1)) / 2;
        return new Vector2Int(q, r);
    }

    public static Vector2Int OffsetToAxialPointy(int col, int row)
    {
        int q = col - (row + (row & 1)) / 2;
        int r = row;
        return new Vector2Int(q, r);
    }
}
