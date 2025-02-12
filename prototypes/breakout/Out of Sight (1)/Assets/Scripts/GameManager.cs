using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField] BrickBoard board;
    public static GameManager sharedInstance;
    [SerializeField] Material color1;
    [SerializeField] Material color2;
    [SerializeField] Material color3;
    [SerializeField] Material color4;
    [SerializeField] Material color5;

    [SerializeField] GameObject ballprefab;

    [SerializeField] GameObject swarmprefab;
    [SerializeField] GameObject brickRingsPrefab;

    Material[] colors = new Material[5];

    int lives = 3;
    int score = 500;
    int maxBricks;
    bool gameEnded = false;
    bool gameStarted = false;
    bool gamePaused = false;
    public int brickAmount = 1;
    int brickHealth = 1;

    GameObject ball;
    GameObject swarm;
    GameObject rings;

    public TMP_Text AddBrickText;
    public TMP_Text AddBrickHealthText;

    int clusterColumns;
    int clusterRows;


    bool upgradeScreen = false;
    bool gameWon = false;
    bool breakTimer = false;
    int level = 1;
    int levelCount = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int ruleset = 0;

    void Awake(){
        sharedInstance = this;
    }
    void Start()
    {
        if( checkScene("Basic")){
            colors[0] = color1;
            colors[1] = color2;
            colors[2] = color3;
            colors[3] = color4;
            colors[4] = color5;
            for(int i = 0;i<board.getRowCount();i++){
                GameObject row = board.GetComponent<BrickBoard>().getRow(i);
                row.GetComponent<BrickRow>().changeRowColor(colors[i],i);
            maxBricks = board.GetComponent<BrickBoard>().getActiveBricks();
            }
        }
        if(checkScene("AntiBreakout1Prototype") || checkScene("AntiBreakout1TestScene"))
        {
            swarm = Instantiate(swarmprefab, new Vector3(-6,1,0), Quaternion.identity);
            swarm.GetComponent<Cluster>().setInitialPos(new Vector3(-6,1,0));
            swarm.SetActive(false);
        }
        if (checkScene("AntiBreakout1"))
        {
            clusterRows = 1;
            clusterColumns = 1;
            swarm = Instantiate(swarmprefab, new Vector3(-6, 1, 0), Quaternion.identity);
            swarm.GetComponent<Cluster>().setInitialPos(new Vector3(-6, 1, 0));
            swarm.SetActive(false);
        }
        if(checkScene("NewAntiBreakout2") || checkScene("AntiBreakout2Prototype")||checkSceneContains("AB2LV")){
            rings = Instantiate(brickRingsPrefab,transform.position, Quaternion.identity);
            rings.SetActive(false);
        }
        ball = Instantiate(ballprefab,transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Basic")
        {

            int blocksHit = board.GetComponent<BrickBoard>().getActiveBricks();
            if (blocksHit == 0) {
                gameWon = true;
                gameStarted = false;
            }
            score = maxBricks - blocksHit;
        }
        if(checkScene("AntiBreakout1Prototype") || checkScene("AntiBreakout1TestScene"))
        {
            if (swarm.GetComponent<Cluster>().getDefeated() == true)
            {
                gameWon = true;
            }
        }
        if (checkScene("AntiBreakout1")){
            if(swarm.GetComponent<Cluster>().getDefeated()==true)
            {
                if (level > levelCount)
                {
                    gameWon = true;
                }
                else
                {
                    upgradeScreen = true;
                }

                Debug.Log("still defeated");
            }
            AddBrickText.text = "Add Brick      " + brickAmount;
            AddBrickHealthText.text = "Add Brick Health      " + brickHealth;
        }
        if (checkScene("NewAntiBreakout2") || checkScene("AntiBreakout2Prototype")||checkSceneContains("AB2LV")){
            score = 0 - rings.GetComponent<BrickRings>().getActiveCount();
            if(rings.activeSelf && rings.GetComponent<BrickRings>().getActiveCount() == 0){
                gameWon = true;
                gameStarted = false;
            }
        }
        
    }
    public void outOfBounds(){
        if(lives>1){
            resetBall();
        }
        else if(!gameWon){
            gameOver();
        }
    }

    public int getLives(){
        return lives;
    }

    public void resetBall(){
        if (!gameWon && gameStarted)
        {
            ball.GetComponent<Ball>().resetBall();
            lives--;
        }

    }
    public void gameOver(){
        lives--;
        breakTimer = true;
        gameEnded = true;
    }

    public Boolean getGameEnded(){
        return gameEnded;
    }

    public int getScore(){
        return score;
    }

    public bool getGameWon(){
        return gameWon;
    }

    public bool getUpgradeScreen()
    {
        Debug.Log("Upgrade Screen is " + upgradeScreen);
        return upgradeScreen;
    }


    
    public void startGame(){
        gameStarted = true;
        if(checkScene("Basic")){
            gameEnded = false;
            gameWon = false;
            ball.GetComponent<Ball>().resetBall();
            board.GetComponent<BrickBoard>().resetRows();
            lives = 3;
            score = 0;
            UIManager.sharedInstance.startGame();
        }
        if(checkScene("AntiBreakout1"))
        {
            gameWon = false;
            upgradeScreen = false;
            gameEnded = false;
            lives = 1;
            swarm.SetActive(true);
            swarm.GetComponent<Cluster>().resetSwarm();
            ball.GetComponent<Ball>().resetBall();
            //StartCoroutine(Timer(20));
            UIManager.sharedInstance.startGame();
        }
        if(checkScene("AntiBreakout1Prototype") || checkScene("AntiBreakout1TestScene"))
        {
            gameWon = false;
            upgradeScreen = false;
            gameEnded = false;
            lives = 1;
            swarm.SetActive(true);
            swarm.GetComponent<Cluster>().resetSwarm();
            ball.GetComponent<Ball>().resetBall();
            StartCoroutine(Timer(20));
            UIManager.sharedInstance.startGame();
        }
        if(checkScene("NewAntiBreakout2") || checkScene("AntiBreakout2Prototype")){
            rings.SetActive(true);
            rings.GetComponent<BrickRings>().resetRings();
            gameEnded = false;
            gameWon = false;
            ball.SetActive(true);
            ball.GetComponent<Ball>().resetBall();
            lives = 3;
            score = 0;
            UIManager.sharedInstance.startGame();
        }
        if(checkSceneContains("AB2LV")){
            rings.SetActive(true);
            rings.GetComponent<BrickRings>().resetRings();
            gameEnded = false;
            gameWon = false;
            ball.SetActive(true);
            ball.GetComponent<Ball>().resetBall();
            lives = 3;
            score = 0;
            UIManager.sharedInstance.startGame();
        }

    }

    public void continueGame()
    {
        if (checkScene("AntiBreakout1"))
        {
            upgradeScreen = false;
            ball.GetComponent<Ball>().resetBall();
            //ResetRows and Columns
            clusterColumns = brickAmount;
            //UIManager.sharedInstance.resumeGame(); //use scene manager to play a scene
            Destroy(swarm);
            swarm = Instantiate(swarmprefab, new Vector3(-6, 1, 0), Quaternion.identity);
            swarm.GetComponent<Cluster>().setInitialPos(new Vector3(-6, 1, 0));
        }

    }

    public void addBrick()
    {
        if (score > 5)
        {
            brickAmount++;
            score = score - 5;
        }
    }

    public void addBrickHealth()
    {
        if (score > 1)
        {
            brickHealth++;
            score = score - 1;
        }
    }

    public static bool checkScene(string name){
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name == name;
    }
    public static bool checkSceneContains(string name)
    {
        int s = name.Length;
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log(name);
        Debug.Log(currentScene.name.Substring(0,s));
        return currentScene.name.Substring(0,s) == name;
    }

    public void setRuleset(int r){
        this.ruleset = r;
    }
    public int getRuleset(){
        return this.ruleset;
    }

    public bool getGameStarted(){
        return gameStarted;
    }

    public void setBrickHealth(int brickHealth)
    {
        this.brickHealth = brickHealth;
    }

    public int getBrickHealth()
    {
        return this.brickHealth;
    }

    IEnumerator Timer(int missionTime)
    {
        breakTimer = false;
        for (int i = missionTime; i >= 0; i--)
        {
            if (!gameWon && !breakTimer)
            {
                score = i;
                if (!gameWon)
                {
                    score = i;
                }
                else
                {
                    breakTimer = false;
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
            if (!gameWon)
            {
                lives = 0;
                gameOver();
            }
        }
        yield return new WaitForSeconds(1f);
    }

    public void controlScreen(){
        UIManager.sharedInstance.controlScreen();
    }

    public int getRows()
    {
        return clusterRows;
    }
    public int getColumns()
    {
        return clusterColumns;
    }
}
