
using UnityEngine;

public class BrickRow : MonoBehaviour
{
    [SerializeField] GameObject brick1;
    [SerializeField] GameObject brick2;
    [SerializeField] GameObject brick3;
    [SerializeField] GameObject brick4;
    [SerializeField] GameObject brick5;
    [SerializeField] GameObject brick6;
    [SerializeField] GameObject brick7;
    [SerializeField] GameObject brick8;
    [SerializeField] GameObject brick9;

    GameObject[] bricks = new GameObject[9];
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start(){
        bricks[0] = brick1;
        bricks[1] = brick2;
        bricks[2] = brick3;
        bricks[3] = brick4;
        bricks[4] = brick5;
        bricks[5] = brick6;
        bricks[6] = brick7;
        bricks[7] = brick8;
        bricks[8] = brick9;

    }
    public GameObject getBrick(int i){
        if(i<bricks.Length){
            return bricks[i];
        }
        else{
            Debug.Log("Out Of Index brick row request");
            return null;
        }

    }

    public int getBrickCount(){
        return bricks.Length;
    }

    public int getActiveBricks(){
        int count = 0;
        for(int i = 0; i<bricks.Length;i++){
            if(bricks[i].activeSelf){
                count++;
            }
        }
        return count;
    }

    public void changeRowColor(Material color){
        for(int i = 0; i<bricks.Length; i++){
            bricks[i].GetComponent<Brick>().changeColor(color);
        }
    }

    public void changeRowColor(Material color, int soundValue){
        for(int i = 0; i<bricks.Length; i++){
            bricks[i].GetComponent<Brick>().changeColor(color, soundValue);
        }
    }

    public void resetBricks(){
        for(int i = 0; i<bricks.Length;i++){
            bricks[i].SetActive(true);
        }
    }

}
