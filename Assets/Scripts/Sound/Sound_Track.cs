using UnityEngine;
using UnityEngine.Audio;

/*
 * An asset object that store audio track
 * and set the track proerties.
 */
[CreateAssetMenu(menuName = "Sound Track", fileName = "Sound Track need a Name", order = 1)]
public class Sound_Track : ScriptableObject
{
    //Sound Track
    public AudioClip track;

    //Sound Properties:
    public float volume = 0; //Volume 0 -> 1
    public float pitch = 0; // Pitch -3 -> 3
    public int priority = 128; // Priority 0 -> 256

    //Fading Properites:
    public bool Fade_In = false; //A boolean variable for if the track is should fade in when play
    public bool Fade_Out = false; //A boolean variable for if the track is should fade out while play
    public float Fade_In_Translating = 1; //Time it take to fade in
    public float Fade_Out_Translating = 1; //Time it take to fade out

    //Stop, Pause, and Resume Proerites
    public bool StopSource = false; //A boolean variable for if the track is should fade out and then stop
    public bool PauseSource = false; //A boolean variable for if the track is should fade out and then Pause
    public bool ResumeSource = false; //A boolean variable for if the track is should resume and fade-in
    public float Stop_time = 1; //The time fade-out until the sound_track's audio source will stop
    public float Pause_time = 1; //The time fade-out until the sound_track's audio source will pause
    public float Resume_time = 1; //The time fade-in out when the sound_track's audio source will resume
    public float Stop_time_vol = 0; //The volume when the sound_track's Audio Source is fade-out until complete stop
    public float Pause_time_vol = 0; //The volume when the sound_track's Audio Source is fade-out until complete pause
    public float Resume_time_vol = 1; //The volume when sound_track's Audio Source resume and fade-in.

    //Loop Properites:
    public bool looped = false;

    /*
     * A properties is Audio Source for future
     * update.
     */
    public AudioMixerGroup mixer = null;

    //Update Check when variable changes.
    private void OnValidate()
    {
        if (volume < 0)
        {
            volume = 0;
        }
        else if (volume > 1)
        {
            volume = 1;
        }
        if (pitch < -3)
        {
            pitch = -3;
        }
        else if (pitch > 3)
        {
            pitch = 3;
        }
        if (priority < 0)
        {
            priority = 0;
        }
        else if (priority > 256)
        {
            priority = 256;
        }
        if (Fade_In_Translating < 1) { Fade_In_Translating = 1; }
        if (Fade_Out_Translating < 1) { Fade_Out_Translating = 1; }
        if (Stop_time < 1) { Stop_time = 1; }
        if (Pause_time < 1) { Pause_time = 1; }
        if (Resume_time < 1) { Resume_time = 1; }
    }
}
