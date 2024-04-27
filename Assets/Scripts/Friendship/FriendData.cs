using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the Friend Data object, where it stores the affinity value between the player and the NPC.
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
