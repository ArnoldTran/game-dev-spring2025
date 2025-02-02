using Unity.Mathematics;
using UnityEngine;

public class BrickRings : MonoBehaviour
{
    // Array of prefabs for each row
    public GameObject[] brickPrefabs;
    public int numRows = 4;
    public int[] numBricksPerRow;
    public float radiusStep = 1.5f;
    public float brickWidth = 0.4f;
    public float speed = 1f;

    GameObject[,] bricks;
    GameObject[] rows;

    int[] numOfRingsPerRow;

    private int selectedRow = 0; // For mode 2 (switching rows)
    private int activeBricks = 0;

    void Awake()
    {
        this.rows = new GameObject[numRows];
        this.numOfRingsPerRow = new int[numRows];
        this.bricks = generateBricks();
    }

    void Update()
    {
        if (GameManager.sharedInstance.getRuleset() == 1)
        {
            HandleDirectRowRotation(); // Mode 1: Direct row rotation with predefined keys
        }
        else
        {
            HandleRowSelectionMode(); // Mode 2: Switch rows with W/S, rotate with A/D
        }
    }

    GameObject[,] generateBricks()
    {
        GameObject[,] brickList = new GameObject[numRows, 60];
        for (int row = 0; row < numRows; row++)
        {
            float radius = (row + 1) * radiusStep;
            int bricksInThisRow = numBricksPerRow[row];
            GameObject rowParent = new GameObject("Row" + (row + 1));
            rowParent.transform.SetParent(gameObject.transform);
            rowParent.transform.position = Vector3.zero;
            rows[row] = rowParent;

            for (int i = 0; i < bricksInThisRow; i++)
            {
                float angle = (i / (float)bricksInThisRow) * 360f;
                Vector3 position = new Vector3(
                    radius * Mathf.Cos(angle * Mathf.Deg2Rad),
                    radius * Mathf.Sin(angle * Mathf.Deg2Rad),
                    0f
                );

                // Ensure we have a prefab assigned for this row
                if (brickPrefabs.Length > row && brickPrefabs[row] != null)
                {
                    // Select the prefab for this row from the array
                    GameObject brick = Instantiate(brickPrefabs[row], position, Quaternion.identity, rowParent.transform);
                    activeBricks++;

                    // Adjust rotation
                    Vector3 directionToCenter = -position.normalized;
                    float brickAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
                    brick.transform.rotation = Quaternion.Euler(0, 0, brickAngle - 90);
                    brickList[row, i] = brick;
                }
                else
                {
                    Debug.LogWarning("No brick prefab assigned for row " + row);
                }
            }
        }
        return brickList;
    }

    void HandleDirectRowRotation()
    {
        float rotationAmount = speed * Time.deltaTime * 300f;

        // Row 1
        if (Input.GetKey(KeyCode.A)) rows[0].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.D)) rows[0].transform.Rotate(Vector3.back * rotationAmount);

        // Row 2
        if (Input.GetKey(KeyCode.J)) rows[1].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.L)) rows[1].transform.Rotate(Vector3.back * rotationAmount);

        // Row 3
        if (Input.GetKey(KeyCode.A)) rows[2].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.D)) rows[2].transform.Rotate(Vector3.back * rotationAmount);

        // Row 4
        if (Input.GetKey(KeyCode.J)) rows[3].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.L)) rows[3].transform.Rotate(Vector3.back * rotationAmount);
    }

    void HandleRowSelectionMode()
    {
        float rotationAmount = speed * Time.deltaTime * 300f;

        // Switch selected row using W/S
        if (Input.GetKeyDown(KeyCode.W)) selectedRow = Mathf.Clamp(selectedRow - 1, 0, numRows - 1);
        if (Input.GetKeyDown(KeyCode.S)) selectedRow = Mathf.Clamp(selectedRow + 1, 0, numRows - 1);

        // Rotate the selected row using A/D
        if (Input.GetKey(KeyCode.A)) rows[selectedRow].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.D)) rows[selectedRow].transform.Rotate(Vector3.back * rotationAmount);
    }

    public int GetActiveBricks()
    {
        return activeBricks;
    }

    public void resetRings()
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numBricksPerRow[i]; j++)
            {
                if (bricks[i, j] != null)
                {
                    bricks[i, j].SetActive(true);
                }
            }
        }
    }

    public int getActiveCount()
    {
        int activeNumber = 0;

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numBricksPerRow[i]; j++)
            {
                if (bricks[i, j] != null && bricks[i, j].activeSelf)
                {
                    activeNumber++;
                }
            }
        }
        return activeNumber;
    }
}

