using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Menu asset with scene management compement*/
[CreateAssetMenu(menuName = "Menu", fileName ="Menu", order = 2)]
public class Menu : ScriptableObject
{
    //Public Variables
    public static int NumberScene = 5;
    public string[] SceneNames = new string[NumberScene]; //The list of Scene Names.
    public int currently_play = 3;

    //When the game start up
    private void Awake()
    {
        SceneNames[0] = "Home";
        SceneNames[1] = "Tutorial";
        SceneNames[2] = "Setting";
    }
    //Load the scene of the level players is going to play or current level player coming back to play
    public void Play()
    {
        //From the Home Scene to first scene in the game.
        SceneManager.LoadScene(SceneNames[currently_play]);
    }
    //Quit out of the Application
    public void Quit()
    {
        Application.Quit();
    }
    //Load the tutorial scene
    public void Tutorial()
    {
        SceneManager.LoadScene(SceneNames[1]);
    }
    //Load the control scene
    public void Setting()
    {
        SceneManager.LoadScene(SceneNames[2]);
    }
    //Reset the level data.
    public void Reset()
    {
        currently_play = 3;
    }
    //Load the Home Scene
    public void Home()
    {
        SceneManager.LoadScene(SceneNames[0]);
    }
    //Load the next level
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneNames[currently_play]);
        currently_play++;
    }
    public void anyscene(int index)
    {
        SceneManager.LoadScene(SceneNames[index]);
    }
}
