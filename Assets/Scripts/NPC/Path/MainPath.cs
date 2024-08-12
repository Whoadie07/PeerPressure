using System.Collections;
using System.Collections.Generic;
using UnityEditor;
// using UnityEditor.Experimental.GraphView;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the main path that the player will follow throughout the game.
[CreateAssetMenu]
public class MainPath : ScriptableObject
{
    // These booleans are to tell if a player has completed a level or the tutorial
    public bool completion1 = false;
    public bool completion2 = false;
    public bool completion3 = false;
    public bool completion4 = false;
}
