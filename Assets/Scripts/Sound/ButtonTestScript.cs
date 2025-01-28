using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTestScript : MonoBehaviour
{
    [SerializeField] private AudioClip testSound;
    [SerializeField] private AudioClip[] multipleSounds;
    
    public void playSound()
    {
    newSoundManager.instance.playSoundFXClip(testSound,transform,1f);
    }

    public void playRandomSound()
    {
        newSoundManager.instance.playRandomSoundFXClip(multipleSounds,transform,1f);
    }
}
