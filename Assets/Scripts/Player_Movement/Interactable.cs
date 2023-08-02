using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    //Public variables
    public int NpcFriendLevel = 0; //NPC friend level to the player 
    public Animator animator;
    public GameObject npc_server;
    public GameObject npc_GameObject;
    public DialogueNode[] npc_current_DialogueNode = new DialogueNode[100];


    //Start is called before the first frame update
    void Start()
    {
        
    }
}
