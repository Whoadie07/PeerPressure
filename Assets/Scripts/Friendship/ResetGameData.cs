using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// When there's a new game starting, all the NPC affinity is set to 0 and the path_object list is empty.
public class ResetGameData : MonoBehaviour
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
    [SerializeField]
    private ThePressure Pressure;
    [SerializeField]
    private PathList List;
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
        Pressure.Pressure = 0;
        for (int i = 0; i < List.pathObjects.Length; i++)
        {
            List.pathObjects[i] = null;
        }
    }
}
