using NUnit.Framework;
using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject lossPanel;
    [SerializeField] GameObject winPanel;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject controlPanel1;
    [SerializeField] GameObject controlPanel2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        sharedInstance = this;
    }

    void Start()
    {
        winPanel.SetActive(false);
        lossPanel.SetActive(false);
        startPanel.SetActive(true);
        if(GameManager.checkScene("NewAntiBreakout2")){
            controlPanel1.SetActive(false);
            controlPanel2.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLivesText();
        UpdateScoreText();

        if(gameOver()){
            lossPanel.SetActive(true);
        }
        else if(gameWon()){
            winPanel.SetActive(true);
        }
    }

    void UpdateLivesText(){
        livesText.text = "Lives: " + GameManager.sharedInstance.getLives();
    }

    void UpdateScoreText(){
        scoreText.text = "Score: " + GameManager.sharedInstance.getScore();
    }

    bool gameOver(){
        return GameManager.sharedInstance.getGameEnded();
    }

    bool gameWon(){
        return GameManager.sharedInstance.getGameWon();
    }

    public void startGame(){
        winPanel.SetActive(false);
        lossPanel.SetActive(false);
        startPanel.SetActive(false);
    }

    public void controlScreen(){
        if(GameManager.sharedInstance.getRuleset() == 0){
            controlPanel1.SetActive(true);
            controlPanel2.SetActive(false);
        }
        else{
            controlPanel1.SetActive(false);
            controlPanel2.SetActive(true);
        }

    }
    public void controlBack(){
        Start();
    }
}
