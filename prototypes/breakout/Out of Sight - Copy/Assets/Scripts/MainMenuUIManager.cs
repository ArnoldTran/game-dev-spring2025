using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject mainSelectPanel;
    [SerializeField] GameObject prototypeSelectPanel;

    private int state = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainSelectPanel.SetActive(true);
        prototypeSelectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            mainSelectPanel.SetActive(true);
            prototypeSelectPanel.SetActive(false);
        }
        else if (state == 1)
        {
            prototypeSelectPanel.SetActive(true);
            mainSelectPanel.SetActive(false);
        }
        else
        {
            Debug.Log("ERROR MAIN MENU UI STATE IS WRONG");
        }

    }

    public void changeState(int i)
    {
        state = i;
    }
}
