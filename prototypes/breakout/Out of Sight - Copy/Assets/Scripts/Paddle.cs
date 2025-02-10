using UnityEngine;

public class Paddle : MonoBehaviour
{
    int paddleSpeed = 18;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            gameObject.transform.position -= new Vector3(paddleSpeed,0,0)*Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            gameObject.transform.position += new Vector3(paddleSpeed,0,0)*Time.deltaTime;

        }
    }
}
