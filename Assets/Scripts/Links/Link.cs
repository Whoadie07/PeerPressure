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
}
