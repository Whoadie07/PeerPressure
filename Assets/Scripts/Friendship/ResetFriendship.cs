using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFriendship : MonoBehaviour
{
    [SerializeField]
    private FriendData Ava;
    [SerializeField]
    private FriendData Ethan;
    [SerializeField]
    private FriendData Isabella;
    [SerializeField]
    private FriendData Liam;
    [SerializeField]
    private FriendData Mia;
    [SerializeField]
    private FriendData Mason;
    [SerializeField]
    private FriendData Noah;
    [SerializeField]
    private FriendData Olivia;
    [SerializeField]
    private FriendData Owen;
    [SerializeField]
    private FriendData Sophia;
    // Start is called before the first frame update
    void Start()
    {
        Ava.Friend = 0;
        Ethan.Friend = 0;
        Isabella.Friend = 0;
        Liam.Friend = 0;
        Mia.Friend = 0;
        Mason.Friend = 0;
        Noah.Friend = 0;
        Olivia.Friend = 0;
        Owen.Friend = 0;
        Sophia.Friend = 0;
    }
}
