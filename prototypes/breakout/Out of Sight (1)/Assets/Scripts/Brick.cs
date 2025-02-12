
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] SpriteRenderer brickRenderer;
    int soundValue;
    GameManager gm;
    int positionXInSwarm;
    int positionYInSwarm;
    int bounces=1;
    int bouncesLeft = 1;
    public Material baseColor;
    public Material denseColor;

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

    private void OnEnable()
    {
        this.bouncesLeft = bounces;
        if (bouncesLeft > 1)
        {
            changeColor(denseColor);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
           // Debug.Log(bouncesLeft);

            if(bouncesLeft == 1)
            {
                gameObject.SetActive(false);
                
            }
            else
            {
                bouncesLeft--;
                if (bouncesLeft == 1)
                {
                   changeColor(baseColor);
                }
            }
            collision.gameObject.GetComponent<Ball>().getRb().linearVelocity *= 1.1f;
            if(GameManager.checkScene("Basic")){
                 SoundManager.sharedInstance.playSound(soundValue);
            }


            if(GameManager.checkScene("AntiBreakout1") || GameManager.checkScene("AntiBreakout1Prototype")|| GameManager.checkScene("AntiBreakout1TestScene"))
            {
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

    public void setPositionInSwarm(int x, int y)
    {
        positionXInSwarm = x;
        positionYInSwarm = y;
    }

    public GameObject getSwarmParent()
    {
        return swarmParent;
    }

    public int[] getPositionInSwarm()
    {
        return new int[] { positionXInSwarm, positionYInSwarm };
    }

    public void setBounces(int i)
    {
        this.bounces = i;
        this.bouncesLeft = i;
        Debug.Log(this.bouncesLeft);
        if (bouncesLeft > 1)
        {
            changeColor(denseColor);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
            brickRenderer.color = Color.red; // Highlight color when selected
        else
            brickRenderer.color = Color.white; // Default color when deselected
}
}
