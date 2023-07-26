using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    //Public variables
    public int NpcFriendLevel = 0;
    public Animator animator;
    public GameObject npc_server;
    public DialogueNode[] npc_current_DialogueNode = new DialogueNode[100];

    //private int npc_type = 0;

    //Protected Variables and Refereneses


    //Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
