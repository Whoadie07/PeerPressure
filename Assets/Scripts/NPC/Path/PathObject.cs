using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Path", menuName ="Path")]
public class PathObject : ScriptableObject
{
    public bool pathComplete = false;
    public bool pathBegin = false;
    [TextArea(3, 100)] public string path_name;
    [TextArea(3, 100)] public string path_description;

   
}
