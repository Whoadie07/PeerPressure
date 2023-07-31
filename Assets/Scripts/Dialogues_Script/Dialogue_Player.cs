using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Player : MonoBehaviour
{
    //Public Variables
    public DialogueNode rootNode = null;
    public DialogueNode currentNode = null;
    public DialogueNode tmpNode = null;


    //UI
    public GameObject DialogueDisplay;
    public GameObject DialogueText;
    public GameObject[] ListofButton = new GameObject[4];
    public Text[] ListofAnswer = new Text[4];

    //Protected Variables
    protected int DialogueIndex = 0;
    protected float DisplaySecond = 0;
    protected float StartSecond = 0;
    protected bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);         
        }
        DialogueDisplay.SetActive(false);
        DialogueText.SetActive(false);
        isRunning = false;
    }

    /*/ Update is called once per frame
    void Update()
    {
        
    }*/
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
    IEnumerator DialogueClicking()
    {
        
        yield return new WaitForSeconds(1f);
        NextLine();

    }
    public void NextLine()
    {
        if(DialogueIndex + 1 < currentNode.DialogueLineSize()) 
        {
            DialogueIndex++;
        }
        if (DialogueIndex < currentNode.DialogueLineSize() - 1)
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
    private void OnGUI()
    {
        Event e = Event.current;
        if(e.button == 1)
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

    //Player choices an answer
    public void SelectAnswer(int index)
    {
        if(index <= ListofAnswer.Length) { return; }
        DialogueNode cur_select = currentNode.GetAnswerLine(index);
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
