using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
/*TODOS
    * Fix the issue where dialogue conjoins when skipped (Requires stopping/managing the playWord coroutine)
*/

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    // Removed: [SerializeField] private TextAsset jsonFile; // No longer needed here if triggers provide it
    [SerializeField] private float textSpeed = 0.03f;

    [Header("Choices UI")] // Added header for clarity
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private static DialogueManager instance;
    private bool dialogueIsPlaying = false;
    private Coroutine playWordCoroutine; // Added reference to manage typing coroutine
    ItemManager itemManager;

    // --- Public property to check state ---
    public bool IsDialoguePlaying
    {
        get { return dialogueIsPlaying; }
    }
    // ---

    private void Awake()
    {
        if (instance != null && instance != this) // Added check for 'this' instance
        {
            Debug.LogWarning("Found more than one instance of Dialogue manager. Destroying duplicate.");
            Destroy(this.gameObject); // Destroy duplicate
            return; // Stop execution for this duplicate instance
        }
        instance = this;
        // DontDestroyOnLoad(gameObject); // Optional: If manager should persist between scenes
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false); // Start with the panel hidden

        // Initialize choices UI (this part is fine in Start)
        int index = 0;
        choicesText = new TextMeshProUGUI[choices.Length];
        foreach (GameObject choice in choices)
        {
            // Initilize each text field in the buttons.
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            // Choice button starts inactive, is activated by DisplayChoices if needed
            choice.SetActive(false);
            index++;
        }
        itemManager = ItemManager.Instance;
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        // Continue dialogue on Space press (or maybe left mouse click?)
        // Consider GetButtonDown("Submit") or similar for more flexibility
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // If currently typing text, skip to the end instantly (Optional improvement)
            if (playWordCoroutine != null)
            {
                StopCoroutine(playWordCoroutine);
                playWordCoroutine = null; // Clear coroutine reference
                dialogueText.text = currentStory.currentText; // Display full line immediately
                // Need to check for choices *after* skipping the typing animation
                DisplayChoices();
            }
            // If not typing (text fully displayed), continue the story
            else if (currentStory.canContinue) // Only continue if Ink story has more lines
            {
                ContinueStory();
            }
            // If no more lines but choices are available, clicking shouldn't exit.
            // Player needs to click a choice button via MakeChoice(index)

            else if (!currentStory.canContinue && currentStory.currentChoices.Count == 0)
            {
                ExitDialogueMode();
            }
        }

    }

    // --- MODIFIED: Now accepts the Ink file to load ---
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (inkJSON == null)
        {
            Debug.LogError("DialogueManager: Cannot enter dialogue mode with a null Ink JSON file!", this.gameObject);
            return;
        }
        if (dialogueIsPlaying)
        {
            Debug.LogWarning("DialogueManager: Tried to start new dialogue while already playing.", this.gameObject);
            return; // Don't interrupt current dialogue
        }

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true); // Activate the dialogue UI panel

        ContinueStory(); // Start the first line/sequence
    }
    // ---

    private void ExitDialogueMode()
    {
        if (playWordCoroutine != null) // Stop typing if exiting
        {
            StopCoroutine(playWordCoroutine);
            playWordCoroutine = null;
        }

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        ClearChoices();
        currentStory = null; // Clear story reference
    }

    private void ContinueStory()
    {
        // Check if story can continue OR if choices are available (prevents exiting if only choices remain)
        if (currentStory.canContinue)
        {
             if (playWordCoroutine != null) // Stop previous typing if player spams continue
             {
                 StopCoroutine(playWordCoroutine);
             }
             // Store coroutine reference to allow skipping/stopping
             playWordCoroutine = StartCoroutine(playWord(currentStory.Continue(), dialogueText));
             // Display choices associated with the *newly continued* line
             DisplayChoices();
        }
        // If story cannot continue & no choices are left, exit
        else if(currentStory.currentChoices.Count == 0)
        {
            ExitDialogueMode();
        }
         // If story cannot continue BUT choices are available, do nothing here.
         // Wait for player to click a choice button (which calls MakeChoice -> ContinueStory).
    }

    private IEnumerator playWord(string word, TextMeshProUGUI textBox)
    {
        textBox.text = ""; // Clear immediately
        char[] letters = word.ToCharArray(); // More efficient potentially

        foreach (char letter in letters)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        playWordCoroutine = null; // Mark coroutine as finished
        // NOTE: DisplayChoices was moved to *after* calling playWord in ContinueStory
        // because choices often appear *after* a line of dialogue in Ink.
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // Hide all choice buttons initially
        ClearChoices();

        // Check if UI can support the number of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices (" + currentChoices.Count + ") than the UI can support (" + choices.Length + ").", this.gameObject);
            // Potentially only display the max supported choices?
        }

        int index = 0;
        // Display the available choices
        foreach (Choice choice in currentChoices)
        {
             if(index < choices.Length) // Ensure we don't go out of bounds
             {
                choices[index].SetActive(true);
                choicesText[index].text = choice.text;
                index++;
             } else {
                break; // Stop if we run out of UI buttons
             }
        }

        // Select the first choice for navigation if any choices were displayed
        if (currentChoices.Count > 0 && choices.Length > 0)
        {
            StartCoroutine(SelectFirstChoice());
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event system requires clearing first.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame(); // Wait a frame for UI to update
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
         if (!dialogueIsPlaying) return; // Don't process choice if not playing

         // Check if the choice index is valid
         if (choiceIndex < 0 || choiceIndex >= currentStory.currentChoices.Count) {
            Debug.LogError($"Invalid choice index: {choiceIndex}. Available choices: {currentStory.currentChoices.Count}");
            return;
         }

        // Debug.Log($"Choice made: Index {choiceIndex}, Text: {currentStory.currentChoices[choiceIndex].text}"); // More informative log
        // No need to ClearChoices() here, DisplayChoices() handles hiding old ones when called next in ContinueStory

        currentStory.ChooseChoiceIndex(choiceIndex); // Tell Ink which choice was made
        ContinueStory(); // Continue the story after making the choice
    }

    private void ClearChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false); // Simply deactivate choice buttons
        }
    }
}