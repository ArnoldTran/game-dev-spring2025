using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using UnityEditor.UI;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    int clusterSpeed = 18;

    [SerializeField] int rowCount;
    [SerializeField] int columnCount;
    [SerializeField] GameObject brickprefab;

//Assumption: Brick Width and Length is the brickPrefab but must be typed in manually.
    [SerializeField] float brickWidth;
    [SerializeField] float brickLength;

    [SerializeField] float xmaxlimit;
    [SerializeField] float xminlimit;
    [SerializeField] float ymaxlimit;
    [SerializeField] float yminlimit;

    float cooldownLength = 1.0f;

    bool onCooldown = false;

    GameObject[,] bricks;
    
    Vector3 initialPos;

    bool defeated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Creation of bricks
        bricks = createBricks();
        initialPos = gameObject.transform.position;
        Debug.Log("Bricks started" + bricks);
    }

    // Update is called once per frame
    void Update()
    {
        int upMost;
        int downMost;
        int leftMost;
        int rightMost;
        Debug.Log("Bricks is still working " + bricks);
        //movement
        if(GameManager.sharedInstance.getGameStarted()&& !onCooldown){
            movement();
        }
        //gettingTheBounds

        upMost= this.getUpMost();
        downMost = this.getDownMost();
        leftMost = this.getLeftMost();
        rightMost = this.getRightMost();

        //checkingTheBounds
        if(rightMost != -1){
            this.upOutOfBounds(upMost);
            this.downOutOfBounds(downMost);
            this.leftOutOfBounds(leftMost);
            this.rightOutOfBounds(rightMost);
        }
        else{
            defeated = true;
        }

        
    }

    GameObject[,] createBricks(){
        GameObject[,] swarm = new GameObject[rowCount, columnCount];
        for(int i = 0; i<rowCount;i++){
            for(int j = 0;j<columnCount;j++){
                GameObject b = Instantiate(brickprefab,new Vector3(gameObject.transform.position.x+j*brickWidth,gameObject.transform.position.y-i*brickLength,0),Quaternion.identity);
                b.transform.SetParent(gameObject.transform);
                swarm[i,j] = b;
                b.GetComponent<Brick>().setSwarmParent(gameObject);
            }
        }

        return swarm;
    }
    private int getUpMost(){
        bool breakNow = false;
        int upmost = -1;
        for(int i=0;i<rowCount;i++){
            for(int j=0;j<columnCount;j++){
                if(bricks[i,j].activeSelf){
                    upmost = i;
                    breakNow = true;
                    break;
                }
            }
            if(breakNow){
                break;
            }
            
        }
        return upmost;
    }
    private int getDownMost(){
        bool breakNow = false;
        int downMost = -1;
        for(int i=rowCount-1;i>=0;i--){
            for(int j=0;j<columnCount;j++){
                if(bricks[i,j].activeSelf){
                    downMost = i;
                    breakNow = true;
                    break;
                }
            }
            if(breakNow){
                break;
            }
            
        }
        return downMost;
    }
    private int getRightMost(){
        bool breakNow = false;
        int rightMost = -1;
        for(int j=columnCount-1;j>=0;j--){
            for(int i=0;i<rowCount;i++){
                if(bricks[i,j].activeSelf){
                    rightMost = j;
                    breakNow = true;
                    break;
                }
            }
            if(breakNow){
                break;
            }
            
        }
        return rightMost;        
    }
    private int getLeftMost(){
        bool breakNow = false;
        int leftMost = -1;
        for(int j=0;j<columnCount;j++){
            for(int i=0;i<columnCount;i++){
                if(bricks[i,j].activeSelf){
                    leftMost = j;
                    breakNow = true;
                    break;
                }
            }
            if(breakNow){
                break;
            }
            
        }
        return leftMost;        
    }
    private void upOutOfBounds(int upmost){
        //CheckIfoutOfBounds
        if(gameObject.transform.position.y - upmost*brickLength>ymaxlimit){
            Debug.Log("Up PING");
            gameObject.transform.position = new Vector3(transform.position.x,ymaxlimit+upmost*brickLength,0);
        }
    }
    private void downOutOfBounds(int downmost){
        if(gameObject.transform.position.y - (downmost+1)*brickLength<yminlimit){
            Debug.Log("Down PING");
            gameObject.transform.position = new Vector3(transform.position.x,yminlimit+(downmost+1)*brickLength,0);
        }
    }
    private void leftOutOfBounds(int leftmost){

        if(gameObject.transform.position.x + leftmost*brickWidth<xminlimit){
            Debug.Log("Left PING");
            gameObject.transform.position = new Vector3(xminlimit-leftmost*brickWidth,gameObject.transform.position.y,0);
        }
    }
    private void rightOutOfBounds(int rightmost){
        if(gameObject.transform.position.x + (rightmost+1)*brickWidth>xmaxlimit){
            Debug.Log("Right PING");
            gameObject.transform.position = new Vector3(xmaxlimit-(rightmost+1)*brickWidth,gameObject.transform.position.y,0);
        }
    }
    void movement(){
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            gameObject.transform.position -= new Vector3(clusterSpeed, 0,0)*Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            gameObject.transform.position += new Vector3(clusterSpeed, 0,0)*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += new Vector3(0, clusterSpeed, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position -= new Vector3(0, clusterSpeed, 0) * Time.deltaTime;
        }
    }
    public void StartCooldown(){
        if(!onCooldown){
            onCooldown = true;
            StartCoroutine(CooldownInitiated());
        }
    }
    IEnumerator CooldownInitiated(){
        yield return new WaitForSeconds(cooldownLength);
        onCooldown = false;
    }
    public void resetSwarm(){
        defeated = false;
        for(int i=0;i<rowCount;i++){
            for(int j=0;j<columnCount;j++){
                Debug.Log("RESET!");
                bricks[i,j].SetActive(true);
            }
        }
        gameObject.transform.position = initialPos;
    }
    public void setInitialPos(Vector3 pos){
        initialPos = pos;
    }
    public bool getDefeated(){
        return defeated;
    }
}
