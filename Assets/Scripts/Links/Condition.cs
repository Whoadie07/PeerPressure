using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When completing a quest, it will apply a condition to a character that would change the root dialogue in the next level.
[CreateAssetMenu]
public class Condition : ScriptableObject
{
    [SerializeField]
    private int _condition;

    public NarrativeNode root1;

    public NarrativeNode root2;
    
    public int Changer
    {
        get { return _condition; }
        set { _condition = value; }
    }
}
