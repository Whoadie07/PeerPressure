using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Chapters", fileName = "Chapters")]
public class ChapterSelect : ScriptableObject
{
    public static int chapterNumber = 3;
    public string[] chapterNames = new string[chapterNumber]; //The list of Scene Names.

    private void Awake()
    {
        chapterNames[0] = "Classroom";
        chapterNames[1] = "Cafeteria";
        chapterNames[2] = "Playground";
    }

    public void Classroom()
    {
        new WaitForSeconds(10);
        SceneManager.LoadScene(chapterNames[0]);
    }

    public void Cafeteria()
    {
        new WaitForSeconds(10);
        SceneManager.LoadScene(chapterNames[1]);
    }

    public void Playground()
    {
        new WaitForSeconds(10);
        SceneManager.LoadScene(chapterNames[2]);
    }
}
