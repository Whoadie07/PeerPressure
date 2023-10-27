using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{

    //Animtor
    public Animator npc_animator;

    //Variables for controll npc interact
    public bool IsInteracting = false;
    public GameObject InteractTarget;
    public NarrativeReader npcreader;
    public NarrativeNode NPC_Dialogue = null;

    //NPC Movement by move them with a path to follow
    public int NpcPath = 0;
    public int CurrentpPath = 0;
    public static int PathNumber = 2;
    public GameObject[] NpcTargetPath = new GameObject[PathNumber]; //The last element final destraction
    public bool KeepMovement = false;
    protected Vector3 cur_path;

    //NPC Control
    public GameObject game_npc_agent;
    public NavMeshAgent npc_agent;

    // Start is called before the first frame update
    void Start()
    {
        NpcPath = 0;
        cur_path = NpcTargetPath[NpcPath].GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        //Make the character move around. 
        if (!IsInteracting)
        {
            Vector3 npc_target = cur_path;
            npc_target.Set(npc_target.x, npc_agent.destination.y, npc_target.z);
            npc_agent.SetDestination(npc_target);
            cur_path.y = npc_agent.destination.y;
            if (game_npc_agent.GetComponent<Transform>().position.Equals(cur_path))
            {
                NpcPath++;
                if (KeepMovement && NpcPath + 1 > PathNumber)
                {
                    NpcPath = 0;
                }
                cur_path = NpcTargetPath[NpcPath].GetComponent<Transform>().position;
            }
        }
        else
        {
            cur_path = transform.position;
        }
        //For the when the NPC stop interacting with the character. 
        if (InteractTarget != null && !IsInteracting)
        {
            if (InteractTarget.GetComponent<Player_Movement>() != null)
            {
                InteractTarget.GetComponent<Player_Movement>().NpcInteracting = false;
                InteractTarget.GetComponent<Player_Movement>().interactable = null;
                InteractTarget = null;
            }
        }
    }
    //NPC will update to communicate with the player. 
    public void UpdateNPC()
    {
        
        npcreader.rootNode = NPC_Dialogue;
        npcreader.currentNode = NPC_Dialogue;
        npcreader.DialoguePlay();
        npcreader.NarrativeObject = this.gameObject;
    }
}
