using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static SimpleConditionalConversation scc;
	public CameraZoom cameraZoom;

    // NOTE: When you do not use the google sheet option, it is expecting the file
    // to be named "data.csv" and for it to be in the Resources folder in Assets.
    public bool useGoogleSheet = false;
    public string googleSheetDocID = "";

    public Transform player;  // Reference to the player
    public List<Transform> npcs = new List<Transform>(); // List of NPCs
    public float interactDistance = 2f; // Distance at which the player can interact with an NPC

    private Transform closestNPC = null;

    // Start is called before the first frame update
    void Start()
    {
		cameraZoom = Camera.main.GetComponent<CameraZoom>();
        if (useGoogleSheet)
        {
            // This will start the asynchronous calls to Google Sheets, and eventually
            // it will give a value to scc, and also call LoadInitialHistory().
            GoogleSheetSimpleConditionalConversation gs_ssc = gameObject.AddComponent<GoogleSheetSimpleConditionalConversation>();
            gs_ssc.googleSheetDocID = googleSheetDocID;
        }
        else
        {
            scc = new SimpleConditionalConversation("data");
            LoadInitialSCCState();
        }
    }

    public static void LoadInitialSCCState()
    {
        // Example of setting the initial state:
        //scc.setGameStateValue("playerWearing", "equals", "Green shirt");
    }

    // Update is called once per frame
    void Update()
    {
        // Find the closest NPC to the player
        closestNPC = FindClosestNPC();

        // If there is a closest NPC and the player is within interaction distance
        if (closestNPC != null && Vector2.Distance(player.position, closestNPC.position) < interactDistance)
        {
			// Debug: Log the name of the closest NPC
            //Debug.Log("Closest NPC: " + closestNPC.name);

            // Show spacebar image (if any), and allow spacebar to trigger dialogue
            if (Input.GetKeyDown(KeyCode.Space))
            {
				// Zoom in on the character
                cameraZoom.ZoomInOnCharacter(closestNPC);

				string line = DialogueManager.scc.getSCCLine(closestNPC.name);
				GameManager.Instance.DisplayText(closestNPC.name, line);
                // Trigger dialogue for the closest NPC
                Debug.Log("Interacted with, " + closestNPC.name);
                string npcName = closestNPC.name;  // Assuming NPCs have unique names in the scene
                if (scc != null)
                {
                    string npcDialogue = DialogueManager.scc.getSCCLine(npcName);  // Replace npcName dynamically
                    Debug.Log(npcDialogue);  // For testing, print dialogue in the console
                }
            }
        }
    }

	public void EndDialogue()
    {
        // Zoom out when the dialogue ends
        cameraZoom.ZoomOut();
    }


    // Method to find the closest NPC
    Transform FindClosestNPC()
    {
        Transform closest = null;
        float minDistance = interactDistance;

        foreach (Transform npc in npcs)
        {
            float distance = Vector2.Distance(player.position, npc.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = npc;
            }
        }
        return closest;
    }
}
