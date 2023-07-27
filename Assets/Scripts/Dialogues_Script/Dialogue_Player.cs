using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Player : MonoBehaviour
{
    //Public Variables
    public DialogueNode rootNode = null;
    public DialogueNode currentNode = null;


    //UI
    public GameObject DialogueDisplay;
    public GameObject[] ListofButton = new GameObject[4];
    public Text[] ListofAnswer = new Text[4];

    //Protected Variables
    protected int DialogueIndex = 0;
    protected float DisplaySecord = 0;
    protected bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ListofButton.Length; i++)
        {
            ListofButton[i].SetActive(false);         
        }
        DialogueDisplay.SetActive(true);
        isRunning = false;
    }

    /*/ Update is called once per frame
    void Update()
    {
        
    }*/
    public void DialoguePlay()
    {
       DialogueIndex = 0;
       DisplaySecord = currentNode.beginFrame;
       if(currentNode.IsPlay())
       {
            StartCoroutine(DialoguePlaying());
       }
    }
    IEnumerator DialoguePlaying()
    {
        yield return new WaitForSeconds(0.01f);
    }
    public void NextLine()
    {
        DialogueIndex++;
    }
    private void OnGUI()
    {
        Event e = Event.current;
        if(e.isMouse && isRunning)
        {
            NextLine();
        }
    }
}
