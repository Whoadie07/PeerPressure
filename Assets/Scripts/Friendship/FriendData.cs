using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FriendData : ScriptableObject
{
    [SerializeField]
    private int _friend;

    public int Friend
    {
        get { return _friend; }
        set { _friend = value; }
    }
}
