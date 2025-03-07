using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
 A Narrative player the when a player interact with object with 
 Narrative Node and Interactable script. A Narrative Player will 
 take the narrative node and player out the narrative dialogue are 
 pre-written in dialogue line
 */
public class NarrativeReader : MonoBehaviour
{
    //Public Variables

    //Narrative Node Variables
    public NarrativeNode rootNode = null; // Root node of the start narrative node.
    /// </summary>
    public NarrativeNode currentNode = null; //Currentnode current narrative node playing. 
    public NarrativeNode tmpNode = null; //tmpNode is using store a preious node for checking. 

    //UI
    public GameObject DialogueDisplay; //Display the Border the text will display.
    public GameObject DialogueName; // Display the person's name.
    public GameObject DialogueText; //The Text Object.
    public GameObject DialogueImage; // The person's picture.
    public GameObject[] ListofButton = new GameObject[4]; //The List of Button for answer
    public Text[] ListofAnswer = new Text[4]; //The list of answers text for the option button option

    //Object Pass with the narrative node.
    public GameObject NarrativeObject;

    /*Protected Variables*/

    //A variable to keep track of second translate of each dialogues.
    public int DialogueIndex = 0;
    protected float DisplaySecond = 0;
    protected float StartSecond = 0;
    protected bool isRunning = false;
    protected bool isFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        //Set the Dialogue UI to inactive before the player interact with the object.
        for (int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);
        }
        DialogueDisplay.SetActive(false);
        DialogueName.SetActive(false);
        DialogueText.SetActive(false);
        isRunning = false;
    }
    //(Fixed) Move to the next dialgoue nextline
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && currentNode != null)
        {
            NextLine();
        }
    }
    //The main function to play the dialogue for each interaction
    public void DialoguePlay()
    {
        DialogueIndex = 0;
        DisplaySecond = currentNode.beginFrame;
        StartSecond = currentNode.beginFrame;
        DialogueDisplay.SetActive(true);
        DialogueName.SetActive(true);
        DialogueName.GetComponent<Text>().text = string.Empty;
        DialogueText.SetActive(true);
        DialogueText.GetComponent<Text>().text = string.Empty;
        if (currentNode.IsPlay())
        {
            isRunning = true;
            DialogueName.GetComponent<Text>().text = currentNode.GetDialogueName(DialogueIndex);
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
            DisplaySecond += currentNode.GetDialogueSecond(DialogueIndex);
            StartCoroutine(DialoguePlaying());
        }
        else
        {
            isRunning = false;
            DialogueName.GetComponent<Text>().text = currentNode.GetDialogueName(DialogueIndex);
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
        }
        if (currentNode.DialogueLineSize() < 1)
        {
            for (int i = 0; i < ListofButton.Length; i++)
            {
                ListofButton[i].SetActive(false);
            }
            DialogueDisplay.SetActive(false);
            DialogueName.SetActive(false);
            DialogueText.SetActive(false);
            isRunning = false;
        }
        if (currentNode.DialogueLineSize() <= 1 && currentNode.IsAQueation)
        {
            for (int i = 0; i < currentNode.AnswerResponseSize(); i++)
            {
                ListofAnswer[i].text = string.Empty;
                ListofAnswer[i].text = currentNode.GetAnswerLine(i).GetAnswer();
                ListofButton[i].SetActive(true);
            }
        }
    }
    //This function will play the dialogue along with animation
    IEnumerator DialoguePlaying()
    {
        while (true)
        {
            if (StartSecond >= currentNode.endFrame)
            {
                break;
            }
            if (StartSecond >= DisplaySecond)
            {
                NextLine();
            }
            yield return new WaitForSeconds(1f);
            StartSecond++;
        }
    }
    //The function play the next line of dialogue in ListofDialogue
    public void NextLine()
    {
        DialogueIndex++;
        if (DialogueIndex < currentNode.DialogueLineSize())
        {
            DialogueName.GetComponent<Text>().text = string.Empty;
            DialogueName.GetComponent<Text>().text = currentNode.GetDialogueName(DialogueIndex);
            DialogueText.GetComponent<Text>().text = string.Empty;
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
            DisplaySecond += currentNode.GetDialogueSecond(DialogueIndex);
            if (DialogueIndex == currentNode.DialogueLineSize() - 1 && currentNode.IsQuestion())
            {
                for (int i = 0; i < currentNode.AnswerResponseSize(); i++)
                {
                    ListofButton[i].SetActive(true);
                    ListofAnswer[i].text = string.Empty;
                    ListofAnswer[i].text = currentNode.GetAnswerLine(i).GetAnswer();
                }
            }
        }
        else
        {
            if (!currentNode.IsQuestion())
            {
                if (NarrativeObject != null) 
                { 
                    NarrativeObject.GetComponent<NPC_Movement>().IsInteracting = false; 
                    NarrativeObject = null; 
                }
                DialogueDisplay.SetActive(false);
                DialogueName.SetActive(false);
                DialogueText.SetActive(false);
                for (int i = 0; i < ListofAnswer.Length; i++)
                {
                    ListofButton[i].SetActive(false);
                }
            }
        }
    }

    //Player choices an answer
    public void SelectAnswer(int index)
    {
        if (index > ListofAnswer.Length) { return; }
        NarrativeNode cur_select = currentNode.GetAnswerLine(index);
        for (int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);
        }
        if (cur_select.IsCorrect())
        {
            currentNode = currentNode.GetAnswerLine(index);
            DialoguePlay();
            if (currentNode.isTherePath)
            {
                GameObject s_object = NarrativeObject.GetComponent<NPC_Movement>().InteractTarget;
                if(s_object != null)
                {
                    int i_loop = -1;
                    for (int i = 0; i < s_object.GetComponent<Player_Movement>().playerPath.pathObjects.Length; i++)
                    {
                        i_loop = i;
                        if (s_object.GetComponent<Player_Movement>().playerPath.pathObjects.ElementAt<PathObject>(i) == null) { break; }
                        else if (s_object.GetComponent<Player_Movement>().playerPath.pathObjects.ElementAt<PathObject>(i).path_name == currentNode.PathObject.path_name) { 
                            i_loop = -1;
                            break;
                        }
                    }
                    if (i_loop != -1)
                    {
                        NarrativeObject.GetComponent<NPC_Movement>().InteractTarget.GetComponent<Player_Movement>().playerPath.pathObjects[i_loop] = currentNode.PathObject;
                        NarrativeObject.GetComponent<NPC_Movement>().InteractTarget.GetComponent<Player_Movement>().index = i_loop;
                        NarrativeObject.GetComponent<NPC_Movement>().CurrentPath = currentNode.PathObject;
                        NarrativeObject.GetComponent<NPC_Movement>().CurrentPath.pathBegin = false;
                    }
                    
                }
            }
        }
        else
        {
            tmpNode = currentNode;
            currentNode = currentNode.GetAnswerLine(index);
            DialoguePlay();
            StartCoroutine(WrongAnswer());
        }
    }
    //If the player get the animation wrong. This function will play out the dialouge for the scene
    IEnumerator WrongAnswer()
    {
        float cur_node_frame = 0f;
        float cur_node_end = tmpNode.endFrame;
        while (cur_node_frame < cur_node_end)
        {
            yield return new WaitForSeconds(1f);
            cur_node_frame += 1;
        }
        //Do something after the animation for the wrong answer select.
        currentNode = tmpNode;
        DialogueIndex = currentNode.DialogueLineSize() - 2;
        NextLine();

    }
}
