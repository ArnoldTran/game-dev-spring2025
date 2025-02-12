
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ball")){
            Debug.Log("Ball out of Bounds!");
            collision.gameObject.SetActive(false);
            GameManager.sharedInstance.outOfBounds();
        }

    }
}
