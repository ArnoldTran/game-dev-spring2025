using Unity.Mathematics;
using UnityEngine;

public class BrickRings : MonoBehaviour
{
    public GameObject brickPrefab;
    public int numRows = 4;
    public float radiusStep = 1.5f;
    public float brickWidth = 1.0f;
    public float speed = 1f;

    GameObject[,] bricks;
    GameObject[] rows;

    int[] numOfRingsPerRow;

    private int selectedRow = 0; // For mode 2 (switching rows)
    private int activeBricks = 0;

    void Awake()
    {
        //Assumption made for 4 rows here
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

    GameObject[,] generateBricks(){
        GameObject[,] brickList = new GameObject[numRows,60];
        for (int row = 0; row < numRows; row++)
        {
            float radius = (row + 1) * radiusStep;
            int bricksInThisRow = Mathf.FloorToInt((2 * Mathf.PI * radius) / brickWidth);
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

                GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity,rowParent.transform);
                activeBricks++;

                // Adjust brick size and rotation
                brick.transform.localScale = new Vector3(0.4f, 0.5f, 1.0f);
                Vector3 directionToCenter = -position.normalized;
                float brickAngle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
                brick.transform.rotation = Quaternion.Euler(0, 0, brickAngle - 90);
                brickList[row,i] = brick;
            }
        }
        return brickList;
    }

    void HandleDirectRowRotation()
    {
        float rotationAmount = speed * Time.deltaTime * 300f;

        // Row 1
        if (Input.GetKey(KeyCode.Q)) rows[0].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.E)) rows[0].transform.Rotate(Vector3.back * rotationAmount);

        // Row 2
        if (Input.GetKey(KeyCode.U)) rows[1].transform.Rotate(Vector3.forward * rotationAmount);
        if (Input.GetKey(KeyCode.O)) rows[1].transform.Rotate(Vector3.back * rotationAmount);

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

    public void resetRings(){//Assumption of row number each time 15,30,45,60
        for(int i = 0; i<numRows;i++){
            for(int j=0;j<(i+1)*15;j++){
                bricks[i,j].SetActive(true);
            }
        }
    }

    public int getActiveCount(){
        int activeNumber = 0;
        for(int i = 0; i<numRows;i++){
            for(int j=0;j<(i+1)*15;j++){

                if(bricks[i,j].activeSelf){
                    activeNumber++;
                }
            }
        }
        return activeNumber;
    }
}

