using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Sound Track", fileName ="Sound Track need a Name", order = 1)]
public class Sound_Track : ScriptableObject
{
    //Sound Track
    public AudioClip track;

    //Sound Properties:
    public float volume = 0;
    public float pitch = 0;
    public int priority = 128;

    //Fading Properites:
    public bool Fade_In = false;
    public bool Fade_Out = false;
    public float Fade_In_Translating = 1;
    public float Fade_Out_Translating = 1;

    //Loop Properites:
    public bool looped = false;

    /*
     * A properties is Audio Source for future
     * update.
     */
    public AudioMixerGroup mixer = null;

    private void OnValidate()
    {
        if (volume < 0)
        {
            volume = 0;
        }else if (volume > 1)
        {
            volume = 1;
        }
        if(pitch < -3)
        {
            pitch = -3;
        }else if(pitch > 3) { 
            pitch = 3;
        }
        if (priority < 0) { 
            priority = 0;
        }else if(priority > 256) { 
            priority = 256;
        }
        if(Fade_In_Translating < 1) { Fade_In_Translating = 1; }
        if(Fade_Out_Translating < 1) { Fade_Out_Translating = 1; }    
    }
}
