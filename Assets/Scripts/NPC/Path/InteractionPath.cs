using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Interaction Path", menuName = "Interaction Path")]
public class InteractionPath : PathObject
{
    // This is the target name. The target name is to identify which name the player has to talk to during the interaction quest.
    public string TargetName;
    // This is where the friend data of the interaction quest giver goes to.
    [SerializeField]
    private FriendData FriendData;
    // This is a list of NPCs the player has to interact with in the quest. The order goes from first to last.
    [SerializeField]
    private string[] QuestNames;
    // This is where the friend data of the NPCs in the quest goes. Also ordering from first to last.
    [SerializeField]
    private FriendData[] FriendList;
    // This is where all the climax dialogues are stored.
    [SerializeField]
    private NarrativeNode[] dialogueClimaxes;
    // This is where the new climaxes are stored.
    [SerializeField]
    private NarrativeNode[] newClimaxes;
    // This is where the wait dialogue for the quest giver is stored.
    public NarrativeNode dialogueWait;
    // This is where the end path dialogue for the quest giver is stored.
    public NarrativeNode dialogueEndPath;
    // This is where the new root node of the quest giver is stored.
    public NarrativeNode replacementRoot;
    // This is where the root nodes of the NPCs are stored during the quest.
    private NarrativeNode[] previousRoots = new NarrativeNode[0];
    // If there is a condition that affects the nodes in other levels
    public Condition condition;
    // This checks if the player has interacted with the NPC or not.
    private bool hasInteracted = false;
    // This is to check if the player has interacted with all of the NPCs in the list.
    private bool[] checker;
    // This checks if the path has been set up or not.
    private bool hasSetUp = false;

    // This starts the quest.
    public void Begin()
    {
        hasInteracted = false;
        hasSetUp = false;
        pathComplete = false;
        if (condition != null) condition.Changer = 0;
    }

    // This updates the dialogue by calling the three functions below depending on the conditions.
    public void UpdateDialogue(NPC_Movement npc, Link link)
    {
        // It checks whether the dialogues for the interaction quest are set up or not.
        if (hasSetUp)
        {
            // This checks whether the player has interacted with all the NPCs in the list or not.
            if (npc.NPC_name == TargetName) UpdateClimaxList(link);
        }
        // If it's not set up, then SetLinksList gets called.
        else SetLinksList(link);
    }

    // This sets up the links for the quest.
    public void SetLinksList(Link link)
    {
        // The waiting dialogue gets set for the quest giver.
        link.GetObject(NPC_Name).GetComponent<NPC_Movement>().NPC_Dialogue = dialogueWait;
        // Then, the roots of the NPCs the player needs to interact during the quest are stored in the previousRoots array.
        previousRoots = new NarrativeNode[FriendList.Length];
        // The checker array is initialized to tell what NPCs the players has interacted.
        checker = new bool[FriendList.Length];
        for (int i = 0; i < FriendList.Length; i++)
        {
            if (previousRoots[i] == null)
            {
                // This uses the link to get the NPC_Movement components of the NPCs in the quest to change the dialogue and save their previous roots.
                previousRoots[i] = link.GetObject(QuestNames[i]).GetComponent<NPC_Movement>().NPC_Dialogue;
                link.GetObject(QuestNames[i]).GetComponent<NPC_Movement>().NPC_Dialogue = dialogueClimaxes[i];
                // The NPCs are also on the same path as the player.
                link.GetObject(QuestNames[i]).GetComponent<NPC_Movement>().CurrentPath = this;
                checker[i] = false;
            }
        }
        // The target name variable is initialized to the first NPC name on the questnames list.
        TargetName = link.GetObject(QuestNames[0]).GetComponent<NPC_Movement>().NPC_name;
        // This marks that the link set up is complete.
        hasSetUp = true;
    }

    // This updates the climaxes of the NPCs
    public void UpdateClimaxList(Link link)
    {
        // This loops through the questnames array to find out if the name the NPC is interacted with matches the target name.
        for (int i = 0; i < FriendList.Length; i++)
        {
            // If so, then the NPC's dialogue gets changed to a new climax dialogue.
            if (link.GetObject(TargetName).GetComponent<NPC_Movement>().NPC_name == QuestNames[i])
            {
                link.GetObject(TargetName).GetComponent<NPC_Movement>().NPC_Dialogue = newClimaxes[i];
                // The checker marks the NPC interaction true.
                checker[i] = true;
                // If there are still some NPCs that need to be interacted, then the target name is set to the next NPC in the list.
                if (i < FriendList.Length - 1) TargetName = QuestNames[i + 1];
            }
        }
        // This checks whether all NPCs have been interacted with the player.
        int counter = 0;
        for (int i = 0; i < checker.Length; i++)
        {
            if (checker[i] == true) counter++;
        }
        // If the player has interacted with all the NPCs on the list, then the dialogue for the giver is changed to the ending dialogue.
        if (counter == FriendList.Length)
        {
            link.GetObject(NPC_Name).GetComponent<NPC_Movement>().NPC_Dialogue = dialogueEndPath;
            hasInteracted = true;
        }
    }

    // This gets called when the player interacts with the quest giver to complete the quest.
    public void CompletedList(Link link)
    {
        // This changes the dialogue of the quest giver to the new root.
        link.GetObject(NPC_Name).GetComponent<NPC_Movement>().NPC_Dialogue = replacementRoot;
        // The affinity is increased by 4.
        FriendData.Friend += 4;
        // Then the NPC's Dialogue return to their original roots.
        for (int i = 0; i < FriendList.Length; i++)
        {
            link.GetObject(QuestNames[i]).GetComponent<NPC_Movement>().NPC_Dialogue = previousRoots[i];
            // The path value is set to null since the quest is over.
            link.GetObject(QuestNames[i]).GetComponent<NPC_Movement>().CurrentPath = null;
            // The NPCs' friend data is increased by 2.
            FriendList[i].Friend += 2;
        }
        // This changes the condition once the path is complete
        if (condition != null) condition.Changer = 1;
        pathBegin = false;
        // This updates the level completion based on what level the path is in
        if (Level == "Tutorial") List.UpdateLevelCompletion(1);
        else if (Level == "Classroom") List.UpdateLevelCompletion(2);
        else if (Level == "Cafeteria") List.UpdateLevelCompletion(3);
        else if (Level == "Playground") List.UpdateLevelCompletion(4);
        pathComplete = true;
    }

    public bool GetInteracted()
    {
        return hasInteracted;
    }
}
