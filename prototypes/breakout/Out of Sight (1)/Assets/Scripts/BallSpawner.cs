using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] GameObject paddle;

    bool spawnAvailable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SPAWNER!!!");
        if(Input.GetKeyDown(KeyCode.Space) && spawnAvailable){
            Debug.Log("PING!");
            GameObject b = Instantiate(ball, paddle.transform.position+paddle.transform.up*-1,Quaternion.identity);
            b.transform.rotation = paddle.transform.rotation;
            b.transform.Rotate(0,0,-90);
            b.GetComponent<Ball>().getRb();
            //b.GetComponent<Ball>().moveBall();
        }
    }

    
}
