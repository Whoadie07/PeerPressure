using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When completing a quest, it will apply a condition to a character that would change the root dialogue in the next level.
[CreateAssetMenu]
public class Condition : ScriptableObject
{
    // The condition is represented as a number of 0 or 1
    [SerializeField]
    private int _condition;

    // These two narrative nodes are to store the different root dialogues and would change the NPC's root dialogue based on if the condition for one of them is met
    public NarrativeNode root1;

    public NarrativeNode root2;
    
    public int Changer
    {
        get { return _condition; }
        set { _condition = value; }
    }
}
