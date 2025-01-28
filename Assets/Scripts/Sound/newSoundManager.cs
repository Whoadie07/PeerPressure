using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class newSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundFXObject;


    //This is a singleton class. Can be called from any other script. Can only be one in each scene
    public static newSoundManager instance;    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Debug.LogWarning("More than one instance of Sound Manager");
        }
    }

    public void playSoundFXClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        //Creates game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assigns audio clip
        audioSource.clip = clip;

        //assigns volume
        audioSource.volume = volume;

        //plays
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        //Destroys gameobject after done playing
        Destroy(audioSource.gameObject,clipLength);
    }

    public void playRandomSoundFXClip(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0,clips.Length);
        //Creates game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        
        //assigns audio clip
        audioSource.clip = clips[rand];

        //assigns volume
        audioSource.volume = volume;

        //plays
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        //Destroys gameobject after done playing
        Destroy(audioSource.gameObject,clipLength);
    }
}
