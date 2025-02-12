using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject lossPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject upgradeScreen;

    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject controlPanel1;
    [SerializeField] GameObject controlPanel2;

    void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {
        winPanel.SetActive(false);
        lossPanel.SetActive(false);
        startPanel.SetActive(true);
        if (GameManager.checkScene("AntiBreakout1"))
        {
            upgradeScreen.SetActive(false);
        }

        if (GameManager.checkScene("NewAntiBreakout2")||GameManager.checkSceneContains("AB2LV"))
        {
            controlPanel1.SetActive(false);
            controlPanel2.SetActive(false);
        }
    }

    void Update()
    {
        UpdateLivesText();
        UpdateScoreText();

        if (gameOver())
        {
            lossPanel.SetActive(true);
        }
        else if (gameWon())
        {
            winPanel.SetActive(true);
        }
        else if (IsUpgradeScreenActive()) // Renamed method
        {
            if (GameManager.checkScene("AntiBreakout2"))
            {
                upgradeScreen.SetActive(true);
            }
        }
        else
        {
            if (GameManager.checkScene("AntiBreakout2"))
            {
                upgradeScreen.SetActive(false);
            }
        }
    }

    void UpdateLivesText()
    {
        livesText.text = "Lives: " + GameManager.sharedInstance.getLives();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + GameManager.sharedInstance.getScore();
    }

    bool gameOver()
    {
        return GameManager.sharedInstance.getGameEnded();
    }

    bool gameWon()
    {
        return GameManager.sharedInstance.getGameWon();
    }

    bool IsUpgradeScreenActive() // Renamed to avoid conflict
    {
        return GameManager.sharedInstance.getUpgradeScreen();
    }

    public void startGame()
    {
        winPanel.SetActive(false);
        lossPanel.SetActive(false);
        startPanel.SetActive(false);
    }

    public void resumeGame()
    {
        //upgradeScreen.SetActive(false);
        //Debug.Log("upgrade screen false");
        //if (!upgradeScreen.activeSelf)
        //{
        //    Debug.Log("Upgrade screen is deactive!");
        //    upgradeScreen.transform.localScale = new Vector3(0f, 0f, 0f);
        //}

    }
    public void displayUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
        upgradeScreen.transform.localScale = new Vector3(1f, 3f, 1f);
        Debug.Log("upgrade screen");

        if (upgradeScreen.activeSelf)
        {
            Debug.Log("Upgrade screen is active!");
        }
  

    }

    public void controlScreen()
    {
        if (GameManager.sharedInstance.getRuleset() == 0)
        {
            controlPanel1.SetActive(true);
            controlPanel2.SetActive(false);
        }
        else
        {
            controlPanel1.SetActive(false);
            controlPanel2.SetActive(true);
        }
    }

    public void controlBack()
    {
        Start();
    }
}
