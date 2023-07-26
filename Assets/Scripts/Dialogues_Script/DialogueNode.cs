using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueNode : ScriptableObject
{
    //Public Variables

    //Start variables
    //Dialogue and Question will be last index
    public static int LineSize;
    [TextArea(3,10000)][SerializeField] public string[] LineOfDialogue = new string[LineSize];
    public bool IsPlaying = false;
    public float[] LineDiaplay = new float[LineSize-1];
    public bool IsAQueation = false;

    public static int ResponseSize;
    public DialogueNode[] AnswerResponse = new DialogueNode[ResponseSize];
    

    //Answer
    [TextArea(3, 10000)][SerializeField] public string answer;
    public bool IsItCorrect = false;
}
