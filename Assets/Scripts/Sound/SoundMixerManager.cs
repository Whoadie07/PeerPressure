using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    /*
                IMPORTANT
        If you want to test/use the linear volume decrease the range needs to be from -80 to 30

        if you want to use log decrease the range needs to be from 0.0001 to 1

    */



    [SerializeField] private AudioMixer audioMixer;



    public void SetMasterVolume(float level)
    {
        /*
        //Linear volume decrease
        audioMixer.SetFloat("Master",level);
        */

        //Log Volume Decrease (Generally sounds better)
        audioMixer.SetFloat("Master",Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        /*
        //Linear volume decrease
        audioMixer.SetFloat("SoundFX",level);
        */

        //Log Volume Decrease (Generally sounds better)
        audioMixer.SetFloat("SoundFX",Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        /*
        //Linear volume decrease
        audioMixer.SetFloat("Music",level);
        */
        
        //Log Volume Decrease (Generally sounds better)
        audioMixer.SetFloat("Music",Mathf.Log10(level) * 20f);
    }
}
