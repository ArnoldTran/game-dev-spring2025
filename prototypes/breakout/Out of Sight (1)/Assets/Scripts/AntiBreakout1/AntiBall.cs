using UnityEngine;

public class AntiBall : MonoBehaviour
{
    
    int initialBallSpeed=3;
    int ballSpeed;
    int maxSpeed = 10;
    [SerializeField]
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

    // public void resetBall(){
    //     gameObject.SetActive(true);
    //     gameObject.transform.position = new Vector3(0,0,0);
    //     gameObject.transform.rotation = Quaternion.identity;
    //     ballSpeed = initialBallSpeed;
    //     float initialDirectionX = Random.Range(-0.8f,0.8f);
    //     rb.linearVelocity = new Vector2(initialDirectionX,-Mathf.Sqrt(1 - (initialDirectionX)*(initialDirectionX)))*ballSpeed;
    // }

    // public void moveBall(){
    //     gameObject.SetActive(true);
    //     rb.linearVelocity = new Vector2(transform.right.x,transform.right.y)*initialBallSpeed;
    // }
    
}
