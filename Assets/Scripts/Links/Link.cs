using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is to link two NPC's together for Interaction paths
public class Link : MonoBehaviour
{
    // These store the game assets in the game screen.
    public GameObject Ava;
    public GameObject Ethan;
    public GameObject Isabella;
    public GameObject Liam;
    public GameObject Mia;
    public GameObject Mason;
    public GameObject Noah;
    public GameObject Olivia;
    public GameObject Owen;
    public GameObject Sophia;

    // These stores the friend data of all the classmate NPCs in the game.
    public FriendData AvaData;
    public FriendData EthanData;
    public FriendData IsabellaData;
    public FriendData LiamData;
    public FriendData MiaData;
    public FriendData MasonData;
    public FriendData NoahData;
    public FriendData OliviaData;
    public FriendData OwenData;
    public FriendData SophiaData;

    // This gets the game object of the NPC
    public GameObject GetObject(string name)
    {
        if (name == "Ava") return Ava;
        else if (name == "Ethan") return Ethan;
        else if (name == "Isabella") return Isabella;
        else if (name == "Liam") return Liam;
        else if (name == "Mia") return Mia;
        else if (name == "Mason") return Mason;
        else if (name == "Noah") return Noah;
        else if (name == "Olivia") return Olivia;
        else if (name == "Owen") return Owen;
        else if (name == "Sophia") return Sophia;
        else return null;
    }
    
    // This gets the friend data of the NPC
    public FriendData GetData(string name)
    {
        if (name == "Ava") return AvaData;
        else if (name == "Ethan") return EthanData;
        else if (name == "Isabella") return IsabellaData;
        else if (name == "Liam") return LiamData;
        else if (name == "Mia") return MiaData;
        else if (name == "Mason") return MasonData;
        else if (name == "Noah") return NoahData;
        else if (name == "Olivia") return OliviaData;
        else if (name == "Owen") return OwenData;
        else if (name == "Sophia") return SophiaData;
        else return null;
    }
}
