using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PathObject : ScriptableObject
{
    //Public Variables
    public PathList List; //This is the variable that connects the path to the main path
    public string NPC_Name = ""; //The name of NPC who assign path.
    public string Level = ""; //The level of which the quest is from.
    public bool pathComplete = false; //Set true if path is complete, or else false;
    public bool pathBegin = false;    //set true if the path is assign, or else false.
    [TextArea(3, 100)] public string path_name; //The name of the path.
    [TextArea(3, 100)] public string path_description; //The description of the path.
}