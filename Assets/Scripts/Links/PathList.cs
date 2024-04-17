using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu]
public class PathList : ScriptableObject
{
    public PathObject[] pathObjects = new PathObject[100];
}
