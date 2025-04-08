using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

// Represents a single cell in the simulation grid
// Handles the cell's state and visual representation
public class CellScript : MonoBehaviour
{
    // References to visual components
    [SerializeField] GameObject selectionPlane;
    [SerializeField] GameObject heightCube;
    private Material heightCubeMaterial;
    [SerializeField] GameObject house;

    Color defaultColor;

    // Cell state with property to update visuals when changed
    public CellState _state = new CellState();

    public CellState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            UpdateVisuals();
        }
    }
    void Start()
    {


        // Cache the material for performance and initialize visuals
        heightCubeMaterial = heightCube.GetComponentInChildren<Renderer>().material;
        defaultColor = heightCubeMaterial.color;
        UpdateVisuals();
    }

    void Update()
    {
        
    }

    void ResetCellState() {
        State.height = 0;
        UpdateVisuals();
    }   

    public void Hover() {
        selectionPlane.SetActive(true);
        // Update the selection plane's position to match the state's height
        float height = transform.position.y + State.height + 0.1f;
        selectionPlane.transform.position = new Vector3(selectionPlane.transform.position.x, height, selectionPlane.transform.position.z);
    }

    public void Unhover() {
        selectionPlane.SetActive(false);
    }

    // Method to enable the HouseButtonEnabled flag (to be called from a button)
 
    public void Clicked() 
    {
       
            State.cellTerrain = CellState.terainType.HOUSE;
            house.SetActive(true);
            Vector3 housePosition = house.transform.position;
            housePosition.y = transform.position.y + State.height + 0.1f;
            house.transform.position = housePosition;
        
    }

    public void RightClicked() {
        
    }   

    // Calculates the next state of this cell for the simulation
    public CellState GenerateNextSimulationStep()
    {
        // Create a copy of the current state to modify
        CellState nextState = this.State.Clone();
        // This is just an example
        ApplyMountainSmoothing(nextState);

        return nextState;
    }

    void ApplyMountainSmoothing(CellState cellState) {
        // Get all neighboring cells (excluding the current cell)
        List<CellScript> neighbors = GridManager.Instance.GetNeighbors(this, true);
        
        // Calculate the average height of all neighboring cells
        float totalHeight = 0;
        foreach (CellScript neighbor in neighbors) {
            totalHeight += neighbor.State.height;
        }
        
        // Set the next height to be the average of all neighbors
        // This creates a smoothing/diffusion effect across the grid
        cellState.height = totalHeight / neighbors.Count;
    }

    // Updates the visual representation of the cell based on its state
    public void UpdateVisuals()
    {
        // Adjust the height cube to match the cell's height value
        if (heightCube != null) {
            heightCube.transform.localScale = new Vector3(1, State.height, 1);
        }

        if (State.pathStateVisuals == "start") {
            heightCubeMaterial.color = Color.white;
        } else if (State.pathStateVisuals == "end") {
            heightCubeMaterial.color = Color.white;
        } else if (State.pathStateVisuals == "open") {
            //heightCubeMaterial.color = Color.green;
        } else if (State.pathStateVisuals == "closed") {
            //heightCubeMaterial.color = Color.red;
        } else if (State.pathStateVisuals == "path") {
            heightCubeMaterial.color = Color.black;
        } else {
            

            if (State.cellTerrain == CellState.terainType.DIRT)
            {
                heightCubeMaterial.color = new Color(0.4f, 0.2f, 0f);
            }
            else //if cube is grass
            {
                heightCubeMaterial.color = new Color(0, 0.5f, 0); //dark green
            }


        }
    }
}
