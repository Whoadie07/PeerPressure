using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainQuest : MonoBehaviour
{
    // These variables are for the MainPath and PathList scriptable objects
    [SerializeField]
    private MainPath Path;
    [SerializeField]
    private PathList List;

    // Update is called once per frame
    // The function marks the level completion once a specific number of paths have been completed
    void Update()
    {
        if (List != null)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (List.tutorial == 2) { Path.completion1 = true; }
                else { Path.completion1 = false; }
            }
            else if (SceneManager.GetActiveScene().name == "Classroom")
            {
                if (List.classroom == 2) { Path.completion2 = true; }
                else { Path.completion2 = false; }
            }
            else if (SceneManager.GetActiveScene().name == "Cafeteria")
            {
                if (List.cafeteria == 4) { Path.completion3 = true; }
                else { Path.completion3 = false; }
            }
            else if (SceneManager.GetActiveScene().name == "Playground")
            {
                if (List.playground == 6) { Path.completion4 = true; }
                else { Path.completion4 = false; }
            }
        }
    }
}
