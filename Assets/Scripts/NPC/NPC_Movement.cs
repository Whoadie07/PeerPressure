using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    // NPC Data
    public string NPC_name = "";

    // Animator
    public Animator npc_animator;

    // Variables for controll npc interact
    public bool IsInteracting = false;
    public GameObject InteractTarget;
    public NarrativeReader npcreader;
    public NarrativeNode NPC_Dialogue = null;
    // This variable is to store the in game link that will connect the NPC to other NPCs
    public GameObject link = null;

    // When completing a path, if a path has a specific condition, this object allows the script to check it to see if there's a specific dialogue needed.
    public Condition condition;

    // This is where the affinity data of the NPC is stored.
    public FriendData NPC_Affinity = null;
    // All NPCs are connected to the pressure levels of the player.
    public ThePressure Peer = null;
    // This is the current path of the NPC.
    public PathObject CurrentPath = null;

    // NPC Movement by move them with a path to follow
    public int NpcPath = 0;
    public int CurrentpPath = 0;
    public static int PathNumber = 2;
    public GameObject[] NpcTargetPath = new GameObject[PathNumber]; //The last element final destraction
    public bool KeepMovement = false;
    protected Vector3 cur_path;

    // NPC Control
    public GameObject game_npc_agent;
    public NavMeshAgent npc_agent;

    // Start is called before the first frame update
    void Start()
    {
        NpcPath = 0;
        cur_path = NpcTargetPath[NpcPath].GetComponent<Transform>().position;
        // This checks the condition and sets the root dialogue based on that condition.
        if (condition != null)
        {
            if (condition.Changer == 1)
            {
                NPC_Dialogue = condition.root1;
            }
            else if (condition.Changer == 0)
            {
                NPC_Dialogue = condition.root2;
            }
        }
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
        // This is for when there's a Interaction Quest going on between the player and the NPC.
        if ((CurrentPath != null)&&(IsInteracting))
        {
            if (CurrentPath.GetType() == typeof(InteractionPath))
            {
                ((InteractionPath)CurrentPath).UpdateDialogue(this, link.GetComponent<Link>());
            }
        }
    }
    // NPC will update to communicate with the player. 
    public void UpdateNPC()
    {
        npcreader.rootNode = NPC_Dialogue;
        npcreader.currentNode = NPC_Dialogue;
        npcreader.DialoguePlay();
        npcreader.NarrativeObject = this.gameObject;
    }
}
