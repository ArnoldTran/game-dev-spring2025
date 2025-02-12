using System.Collections;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    int clusterSpeed = 18;

    //[SerializeField] int rowCount;
    [SerializeField] int columnCount;
    [SerializeField] int rowCount;
    [SerializeField] GameObject brickprefab;
    [SerializeField] GameObject explosiveBrickprefab;
    [SerializeField] GameObject badBrickprefab;
    [SerializeField] GameObject denseBrickprefab;

    //Assumption: Brick Width and Length is the brickPrefab but must be typed in manually.
    [SerializeField] float brickWidth;
    [SerializeField] float brickLength;

    [SerializeField] float xmaxlimit;
    [SerializeField] float xminlimit;
    [SerializeField] float ymaxlimit;
    [SerializeField] float yminlimit;

    float cooldownLength = 0.2f;

    bool onCooldown = false;

    GameObject[,] bricks;

    int badBricks=0;
    
    Vector3 initialPos;

    bool defeated = false;

    int brickHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        brickHealth = GameManager.sharedInstance.getBrickHealth();
        if (GameManager.checkScene("AntiBreakout1"))
        {
            rowCount = GameManager.sharedInstance.getRows();
            columnCount = GameManager.sharedInstance.getColumns();
        }
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
        if (getActiveCount() - badBricks == 0)
        {
            defeated = true;
        }


    }

    GameObject[,] createBricks(){
        GameObject[,] swarm = new GameObject[rowCount, columnCount];
        for(int i = 0; i<rowCount;i++){
            for(int j = 0;j<columnCount;j++){
                //Temp Test for Explosive
                GameObject b;
                //if(i ==2 && j == 2){
                //    b = Instantiate(explosiveBrickprefab, new Vector3(gameObject.transform.position.x + j * brickWidth, gameObject.transform.position.y - i * brickLength, 0), Quaternion.identity);
                //}
                //Temp Test for BadBrick
                //if(i ==2 && j == 2){
                //    b = Instantiate(badBrickprefab, new Vector3(gameObject.transform.position.x + j * brickWidth, gameObject.transform.position.y - i * brickLength, 0), Quaternion.identity);
                //    badBricks++;
                //}
                //else
                //{
               // b = Instantiate(brickprefab, new Vector3(gameObject.transform.position.x + j * brickWidth, gameObject.transform.position.y - i * brickLength, 0), Quaternion.identity);
                //Test for DenseBrick
                b = Instantiate(denseBrickprefab, new Vector3(gameObject.transform.position.x + j * brickWidth, gameObject.transform.position.y - i * brickLength, 0), Quaternion.identity);
                //}
                b.GetComponent<Brick>().setBounces(brickHealth);
                b.transform.SetParent(gameObject.transform);
                swarm[i,j] = b;
                b.GetComponent<Brick>().setSwarmParent(gameObject);
                b.GetComponent<Brick>().setPositionInSwarm(i, j);

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
            for(int i=0;i<rowCount;i++){
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

    public void resetDefeated()
    {
        defeated = false;
    }

    public void explodeBrick(int x, int y)
    {
        Debug.Log("Exploding Brick: " + x + " , " + y);
        for(int i = (x - 1); i < (x + 2); i++)
        {
            for(int j = (y - 1); j < (y + 2); j++)
            {
                if (i >=0 && j>=0 && i<columnCount && j<rowCount)
                {
                    Debug.Log("Turning off Brick");
                    bricks[i, j].SetActive(false);
                }
            }
        }
    }

    public void badBrickHitAlert()
    {
        GameManager.sharedInstance.gameOver();
    }

    public int getActiveCount()
    {
        int count = 0;
        for(int i = 0; i < bricks.GetLength(0); i++)
        {
            for(int j = 0; j < bricks.GetLength(1); j++)
            {
                if (bricks[i,j].gameObject != null && bricks[i, j].gameObject.activeSelf  == true)
                {
                       count++;
                }
            }
        }
        return count;
    }


}
