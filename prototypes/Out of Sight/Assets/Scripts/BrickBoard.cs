using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Purchasing;

public class BrickBoard : MonoBehaviour
{
    [SerializeField] GameObject row1;
    [SerializeField] GameObject row2;
    [SerializeField] GameObject row3;
    [SerializeField] GameObject row4;
    [SerializeField] GameObject row5;

    GameObject[] rows = new GameObject[5];
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rows[0] = row1;
        rows[1] = row2;
        rows[2] = row3;
        rows[3] = row4;
        rows[4] = row5;
    }

    // Update is called once per frame
    public GameObject getRow(int i){
        if(i<rows.Length){
            return rows[i];
        }
        else{
            Debug.Log("Indexing Board Out of bounds");
            return null;
        }

    }

    public int getActiveBricks(){
        int count = 0;
        for(int i = 0; i<rows.Length;i++){
            count += rows[i].GetComponent<BrickRow>().getActiveBricks();
        }
        return count;
    }

    public int getRowCount(){
        return rows.Length;
    }

    public void resetRows(){
        for(int i=0;i<rows.Length;i++){
            rows[i].GetComponent<BrickRow>().resetBricks();
        }
    }
}
