
using UnityEditor;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] GameObject ring4;
    [SerializeField] GameObject ring3;
    [SerializeField] GameObject ring2;
    [SerializeField] GameObject ring1;

    GameObject bricksRing;
    GameObject[] rings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ring1.SetActive(false);
        ring2.SetActive(false);
        ring3.SetActive(false);
        ring4.SetActive(false);

        rings = new GameObject[4];
        rings[0] = ring1;
        rings[1] = ring2;
        rings[2] = ring3;
        rings[3] = ring4;
    }

    // Update is called once per frame
    void Update()
    {
        // if(bricksRing != null){
        //     int activeSelection= bricksRing.GetComponent<BrickRings>().getSelectedRow();
        //     activateRings(activeSelection);
        // }
    }

    void activateRings(int num){
        rings[num].SetActive(true);
        for(int i=0;i<rings.Length;i++){
            if(i == num){
                rings[i].SetActive(true);
            }
            else{
                rings[i].SetActive(false);
            }
        }
    }

    public void setBricksRing(GameObject brickRing){
        bricksRing = brickRing;
    }
}
