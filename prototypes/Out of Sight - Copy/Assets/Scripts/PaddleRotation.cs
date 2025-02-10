using UnityEngine;

public class PaddleRotation : MonoBehaviour
{
    int paddleSpeed = 120;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            gameObject.transform.Rotate(new Vector3(0,0,-1)*paddleSpeed*Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W)){
            gameObject.transform.Rotate(new Vector3(0,0,1)*paddleSpeed*Time.deltaTime);

        }
    }
    }
}

//TO DO: Enemy Spawning and Ball Spawning.