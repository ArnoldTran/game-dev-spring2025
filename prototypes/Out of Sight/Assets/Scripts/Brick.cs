using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] SpriteRenderer brickRenderer;
    int soundValue;
    [SerializeField] GameManager gm;

    GameObject swarmParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundValue = Random.Range(0,4);
        gm = GameManager.sharedInstance;  // Assign the GameManager instance to gm
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ball")){
            Debug.Log("Ball Hit!");


            gameObject.SetActive(false);
            collision.gameObject.GetComponent<Ball>().getRb().linearVelocity *= 1.1f;
            if(GameManager.checkScene("Basic")){
                 SoundManager.sharedInstance.playSound(soundValue);
            }


            if(GameManager.checkScene("AntiBreakout1")){
                swarmCooldown();
            
            }
        }
    }

    public void changeColor(Material color){
        brickRenderer.material= color;
    }
    public void changeColor(Material color, int soundValue){
        brickRenderer.material= color;
        this.soundValue = soundValue;
    }
    public void swarmCooldown(){
        swarmParent.GetComponent<Cluster>().StartCooldown();
    }

    public void setSwarmParent(GameObject g){
        swarmParent = g;
    }

}
