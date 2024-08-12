using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

// This is the path_objects list that will store all the paths that the player accepts in the game.
[CreateAssetMenu]
public class PathList : ScriptableObject
{
    public PathObject[] pathObjects = new PathObject[100];
    // This counts the number of paths completed to tell when the player can move on to the next level.
    public int tutorial;
    public int classroom;
    public int cafeteria;
    public int playground;

    // This function gets called to update the level completion accordingly
    public void UpdateLevelCompletion(int type)
    {
        if (type == 1) { tutorial++; }
        else if (type == 2) { classroom++; }
        else if (type == 3) { cafeteria++; }
        else if (type == 4) { playground++; }
    }
}
