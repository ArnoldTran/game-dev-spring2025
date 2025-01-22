using UnityEngine;

public class PaddleScript : MonoBehaviour
{

    [SerializeField]
    public float speed = 7;
    [SerializeField]
    public float maxX = 7.5f;

    float MovementHorizontal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementHorizontal = Input.GetAxis("Horizontal");
        if ((MovementHorizontal > 0 && transform.position.x<maxX) || (MovementHorizontal<0 && transform.position.x>-maxX))
        {
            transform.position +=Vector3.right*MovementHorizontal*speed*Time.deltaTime;
        }
    }
}
