using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Menu", fileName ="Menu", order = 2)]
public class Menu : ScriptableObject
{
    //Public Variables
    public static int NumberScene;
    public string[] NumberSceneMenu = new string[NumberScene]; //The 
    protected int currently_play = 3; 

    //Load the scene of the level players is going to play or current level player coming back to play
    public void Play()
    {
        //From the 
        SceneManager.LoadScene(NumberSceneMenu[currently_play]);
    }
    //Quit out of the Application
    public void Quit()
    {
        Application.Quit(); 
    }
    //Load the tutorial scene
    public void Tutorial()
    {
        SceneManager.LoadScene(NumberSceneMenu[1]);
    }
    //Load the control scene
    public void Control()
    {
        SceneManager.LoadScene(NumberSceneMenu[2]);
    }
    //Reset the level data.
    public void Reset()
    {
        currently_play = 3;
    }
    //Load the Home Scene
    public void Home()
    {
        SceneManager.LoadScene(NumberSceneMenu[0]);
    }
    //Load the next level
    public void NextLevel()
    {
        SceneManager.LoadScene(NumberSceneMenu[currently_play]);
        currently_play++;
    }
}
