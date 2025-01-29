
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    int initialBallSpeed=3;
    int ballSpeed;
    int maxSpeed = 10;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballSpeed = initialBallSpeed;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(rb.linearVelocity.magnitude>maxSpeed){
            rb.linearVelocity = rb.linearVelocity * (maxSpeed/rb.linearVelocity.magnitude);
        }
        if(rb.linearVelocity.magnitude<initialBallSpeed && rb.linearVelocity.magnitude>0.1){
            rb.linearVelocity = rb.linearVelocity * (initialBallSpeed/rb.linearVelocity.magnitude);
        }
    }

    public Rigidbody2D getRb(){
        return rb;
    }

    public void resetBall(){
        if(GameManager.checkScene("Basic")){
            gameObject.SetActive(true);
            gameObject.transform.position = new Vector3(0,0,0);
            gameObject.transform.rotation = Quaternion.identity;
            ballSpeed = initialBallSpeed;
            float initialDirectionX = Random.Range(-0.8f,0.8f);
            rb.linearVelocity = new Vector2(initialDirectionX,-Mathf.Sqrt(1 - initialDirectionX*initialDirectionX))*ballSpeed;
        }
        else if(GameManager.checkScene("AntiBreakout1")||GameManager.checkScene("NewAntiBreakout2")){
            gameObject.SetActive(true);
            gameObject.transform.position = new Vector3(0,0,0);
            gameObject.transform.rotation = Quaternion.identity;
            ballSpeed = initialBallSpeed;
            setStartingVelocity(ballSpeed);
            }

    }

    void setStartingVelocity(int ballSpeed){
        float initialDirectionX = Random.Range(0.2f,0.8f);
        float initialDirectionY = Mathf.Sqrt(1 - initialDirectionX*initialDirectionX);
        if(Random.Range(0,1) == 0){
            initialDirectionX*=-1;
        }
        if(Random.Range(0,1) == 0)
        {
            initialDirectionY*=-1;
        }
        Debug.Log("X-speed = " + initialDirectionX + "\nY-speed = " + initialDirectionY);
        rb.linearVelocity = new Vector2(initialDirectionX*ballSpeed,initialDirectionY*ballSpeed);
        Debug.Log("XLin: "+ rb.linearVelocityX);
    }
    
}
