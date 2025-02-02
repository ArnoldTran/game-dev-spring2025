using System;
using System.Collections;
using System.Data;
using System.Runtime.ExceptionServices;
using System.Security;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    int score = 0;
    int maxBricks;
    bool gameEnded = false;

    bool gameStarted = false;

    GameObject ball;
    GameObject swarm;

    GameObject rings;
    
    bool gameWon = false;
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
        if(checkScene("AntiBreakout1")){
            swarm = Instantiate(swarmprefab, new Vector3(-6,1,0), Quaternion.identity);
            swarm.GetComponent<Cluster>().setInitialPos(new Vector3(-6,1,0));
            swarm.SetActive(false);
        }
        if(checkScene("NewAntiBreakout2")){
            rings = Instantiate(brickRingsPrefab,transform.position, Quaternion.identity);
            rings.SetActive(false);
        }
        ball = Instantiate(ballprefab,transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Swarm In GAME MANAGER " + swarm);
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Basic")
        {

            int blocksHit = board.GetComponent<BrickBoard>().getActiveBricks();
            if(blocksHit == 0){
                gameWon = true;
                gameStarted = false;
            }
            score = maxBricks - blocksHit;
        }
        if(checkScene("AntiBreakout1")){
            if(swarm.GetComponent<Cluster>().getDefeated()){
                gameWon = true;
                gameStarted = false;
            }
        }
        if(checkScene("NewAntiBreakout2")){
            score = 150-rings.GetComponent<BrickRings>().getActiveCount();
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
        else{
            gameOver();
        }
    }

    public int getLives(){
        return lives;
    }

    public void resetBall(){
        ball.GetComponent<Ball>().resetBall();
        lives--;
    }
    public void gameOver(){
        lives--;
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
        if(checkScene("AntiBreakout1")){
            gameWon = false;
            gameEnded = false;
            lives = 1;
            swarm.SetActive(true);
            swarm.GetComponent<Cluster>().resetSwarm();
            ball.GetComponent<Ball>().resetBall();
            StartCoroutine(Timer(20));
            UIManager.sharedInstance.startGame();
        }
        if(checkScene("NewAntiBreakout2")){
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

    public static bool checkScene(string name){
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name == name;
    }

    public void setRuleset(int r){
        this.ruleset = r;
    }
    public int getRuleset(){
        return this.ruleset;
    }

    public void ToggleRuleset()
{
    this.ruleset = (this.ruleset == 0) ? 1 : 0;
}

    public bool getGameStarted(){
        return gameStarted;
    }

    IEnumerator Timer(int missionTime){
        for(int i=missionTime;i>=0;i--){
            if(!gameWon){
                score = i;
            }
            else{
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        if(!gameWon){
            lives=0;
            gameOver();
        }

    }

    public void controlScreen(){
        UIManager.sharedInstance.controlScreen();
    }
}
