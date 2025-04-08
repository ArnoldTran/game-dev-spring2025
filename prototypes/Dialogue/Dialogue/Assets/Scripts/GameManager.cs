using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern: Static instance that allows access from anywhere in the code
    public static GameManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            Destroy(this);
        }
        else
        {
            GameManager.Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure only one instance of GameManager exists and assign it to the static Instance variable
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances if any
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to display the speaker's name and dialogue
    public void DisplayText(string speaker, string dialogue) 
    {
        nameText.text = speaker;
        dialogueText.text = dialogue;
        dialoguePanel.SetActive(true);
    }
}
