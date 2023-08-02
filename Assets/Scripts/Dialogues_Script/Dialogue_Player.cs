using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 A Dialogue player the when a player interact with object with 
 Dialogue Node and Interactable script. A Dialogue Player will 
 take the Dialogue Node and player out the narrative dialogue 
 pre-written in dialogue line
 */
public class Dialogue_Player : MonoBehaviour
{
    //Public Variables

    //Dialogue Node Variables
    public DialogueNode rootNode = null;
    public DialogueNode currentNode = null;
    public DialogueNode tmpNode = null;


    //UI
    public GameObject DialogueDisplay; //Display the Border the text will display.
    public GameObject DialogueText; //The Text Object.
    public GameObject[] ListofButton = new GameObject[4]; //The List of Button for answer
    public Text[] ListofAnswer = new Text[4]; //The list of answers text for the option button option

    /*Protected Variables*/

    //A variable to keep track of second translate of each dialogues.
    protected int DialogueIndex = 0;
    protected float DisplaySecond = 0;
    protected float StartSecond = 0;
    protected bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        //Set the Dialogue UI to inactive before the player interact with the object.
        for(int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);         
        }
        DialogueDisplay.SetActive(false);
        DialogueText.SetActive(false);
        isRunning = false;
    }

    //The main function to play the dialogue for each interaction
    public void DialoguePlay()
    {
        DialogueIndex = 0;
        DisplaySecond = currentNode.beginFrame;
        StartSecond = currentNode.beginFrame;
        DialogueDisplay.SetActive(true);
        DialogueText.SetActive(true);
        DialogueText.GetComponent<Text>().text = string.Empty;
        if (currentNode.IsPlay())
        {
            isRunning = true;
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
            DisplaySecond += currentNode.GetDialogueSecond(DialogueIndex);
            StartCoroutine(DialoguePlaying());
        }
        else
        {
            isRunning = false;
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
            DisplaySecond += currentNode.GetDialogueSecond(DialogueIndex);
        }
    }
    //This function will play the dialogue along with the animation
    IEnumerator DialoguePlaying()
    {
        while(true)
        {
            if (StartSecond >= currentNode.endFrame)
            {
                break;
            }
            if(StartSecond <= DisplaySecond)
            {
                NextLine();
            }
            yield return new WaitForSeconds(1f);
            StartSecond++;
        }
    }
    //This function play the dialogue, but only by clicking
    IEnumerator DialogueClicking()
    {
        
        yield return new WaitForSeconds(1f);
        NextLine();

    }
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.button == 1)
        {
            if (isRunning)
            {
                NextLine();
            }
            else
            {
                StartCoroutine(DialogueClicking());
            }
        }
    }
    //The function play the next line of dialogue in ListofDialogue
    public void NextLine()
    {
        if(DialogueIndex + 1 < currentNode.DialogueLineSize()) 
        {
            DialogueIndex++;
        }
        if (DialogueIndex < currentNode.DialogueLineSize())
        {
            DialogueText.GetComponent<Text>().text = currentNode.GetDialogueLine(DialogueIndex);
            DisplaySecond += currentNode.GetDialogueSecond(DialogueIndex);
            if (DialogueIndex == currentNode.DialogueLineSize() - 1 && currentNode.IsQuestion())
            {
                for (int i = 0; i < ListofButton.Length; i++)
                {
                    ListofButton[i].SetActive(true);
                    ListofAnswer[i].text = string.Empty;
                    ListofAnswer[i].text = currentNode.GetAnswerLine(i).GetAnswer();
                }
            }
            else if(currentNode.IsQuestion())
            {
                DialogueDisplay.SetActive(false);
                DialogueText.SetActive(false);
            }
        } 

    }

    //Player choices an answer
    public void SelectAnswer(int index)
    {
        if(index <= ListofAnswer.Length) { return; }
        DialogueNode cur_select = currentNode.GetAnswerLine(index);
        for (int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);
        }
        if (cur_select.IsCorrect())
        {
            currentNode = currentNode.GetAnswerLine(index);
            DialoguePlay();
        }
        else
        {
            tmpNode = currentNode;
            currentNode = currentNode.GetAnswerLine(index);
            DialoguePlay();
            StartCoroutine(WrongAnimation());
        }
    }
    //If the player get the animation wrong. This function will play out the dialouge for the scene
    IEnumerator WrongAnimation()
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
