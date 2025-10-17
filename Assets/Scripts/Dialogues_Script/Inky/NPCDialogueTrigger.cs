using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueIcon;
    // --- Each trigger needs its own Ink file reference ---
    [SerializeField] private TextAsset inkJSONFile;
    // ---

    private bool playerInRange = false; // Track if player is in range

    void Start()
    {
        if (dialogueIcon != null)
            dialogueIcon.SetActive(false);
        playerInRange = false;
    }

    void Update()
    {
        // Only check for input if the player is within the trigger area
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.I)) // Or Input.GetButtonDown("Interact")
            {
                AttemptToStartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            // Show icon only if dialogue isn't already playing (optional nice touch)
            DialogueManager manager = DialogueManager.GetInstance();
            if (dialogueIcon != null && (manager == null || !manager.IsDialoguePlaying))
            {
                 dialogueIcon.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialogueIcon != null)
                dialogueIcon.SetActive(false);
            // Decide if you want to EXIT dialogue if the player walks away. Generally NOT recommended.
        }
    }

    // --- Method to handle starting the dialogue ---
    private void AttemptToStartDialogue()
    {
        // Check if the Ink JSON file has been assigned in the Inspector for THIS trigger
        if (inkJSONFile == null)
        {
            Debug.LogWarning($"NPCDialogueTrigger on {this.gameObject.name} doesn't have an Ink JSON file assigned!", this.gameObject);
            return;
        }

        // Get the DialogueManager instance
        DialogueManager manager = DialogueManager.GetInstance();

        // Check if the manager exists AND if dialogue is NOT already playing
        if (manager != null && !manager.IsDialoguePlaying) // Use the public property
        {
            Debug.Log($"Player interacted with {this.gameObject.name}. Attempting to start dialogue: {inkJSONFile.name}");

            // Hide the interaction icon BEFORE starting dialogue
            if (dialogueIcon != null)
                dialogueIcon.SetActive(false);

            // Call the manager's method, passing THIS trigger's specific Ink file
            manager.EnterDialogueMode(inkJSONFile);
        }
        else if (manager == null)
        {
            Debug.LogError("DialogueManager instance is null! Make sure the DialogueManager GameObject is active in the scene.", this.gameObject);
        }
        else
        {
            // Dialogue is already playing, do nothing (or provide feedback)
            Debug.Log($"Player tried to interact with {this.gameObject.name}, but dialogue is already playing.");
        }
    }
    // ---
}