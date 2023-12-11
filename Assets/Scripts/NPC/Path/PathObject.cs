using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PathObject : ScriptableObject
{
    public string NPC_Name = "";
    public bool pathComplete = false;
    public bool pathBegin = false;
    [TextArea(3, 100)] public string path_name;
    [TextArea(3, 100)] public string path_description;
}