using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    //PUblic Variables:

    //List of Audio Source Created
    Hashtable m_audio_source_list = new Hashtable();

    //List of String Name of Audio Source
    ArrayList m_sound_name_list = new ArrayList();

    //The gameobject that user want to create
    public GameObject m_audio_source;

    //Connect the the Sound Dictinoary
    public Sound_Dict sp_dict;

    //Create Audio Source by pass in source name and track number that you user
    //want to play by Audio Source.
    public void CreateAudioSource(string source_name, int track_num)
    {
        GameObject cur_m_audio_source = null;
        //Check if the audio source with name existed.
        if (m_sound_name_list.Contains(source_name))
        {
            cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
            if (SourceCheck(cur_m_audio_source) || BoundCheck(track_num))
            {
                return;
            }
            if (cur_m_audio_source.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Current Audio Source is playing.");
            }
            else
            {
                cur_m_audio_source.GetComponent<Audio_Data>().ad_track = sp_dict.tracks[track_num];
                PlayAudioSource(source_name);
            }
            return;
        }

        //Create a new Audio Sources
        cur_m_audio_source = Instantiate(m_audio_source) as GameObject;
        if (SourceCheck(cur_m_audio_source) || BoundCheck(track_num))
        {
            return;
        }
        cur_m_audio_source.GetComponent<Audio_Data>().ad_track = sp_dict.tracks[track_num];
        m_sound_name_list.Add(source_name);
        m_audio_source_list.Add(source_name, cur_m_audio_source);
        PlayAudioSource(source_name);
    }

    //Play the Create Audio Source by the soruce name
    public void PlayAudioSource(string source_name)
    {
        if (!(m_sound_name_list.Contains(source_name)))
        {
            return;
        }

        GameObject cur_m_audio_source = Instantiate(m_audio_source) as GameObject;
        if (SourceCheck(cur_m_audio_source))
        {
            return;
        }
        Sound_Track cur_track = cur_m_audio_source.GetComponent<Audio_Data>().ad_track;
        if(cur_track != null)
        {
            if (cur_m_audio_source.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Current Audio Source is playing.");
            }
            else
            {
                cur_m_audio_source.GetComponent<AudioSource>().clip = cur_track.track;
                cur_m_audio_source.GetComponent<AudioSource>().pitch = cur_track.pitch;
                cur_m_audio_source.GetComponent<AudioSource>().priority = cur_track.priority;
                if(cur_track.looped) { cur_m_audio_source.GetComponent<AudioSource>().loop = cur_track.looped; }
                cur_m_audio_source.GetComponent<AudioSource>().Play();
                if (cur_track.Fade_In)
                {
                    StartCoroutine(VolumeFade(cur_m_audio_source, cur_track.Fade_In_Translating, cur_track.volume));
                }
                else
                {
                    cur_m_audio_source.GetComponent<AudioSource>().volume = cur_track.volume;
                }

            }  
        }
    }

    //Stop the Audio Source

    //Change the track on Audio Source
    public void ChangeTrack()
    {

    }

    //Fade Functions
    IEnumerator VolumeFade(GameObject sourceObject, float fade_time, float volume_num)
    {
        if (volume_num < 0)
        {
            volume_num = 0;
        }else if (volume_num > 1)
        {
            volume_num = 1;
        }
        float change_time = 0;
        float change_in_volume = (volume_num - sourceObject.GetComponent<AudioSource>().volume) / fade_time;
        while (change_time <= fade_time)
        {
            sourceObject.GetComponent<AudioSource>().volume += change_in_volume;
            yield return new WaitForSeconds(0.1f);
            fade_time++;
        }
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
        {
            volume_num = 1;
        }
    }
    IEnumerator PitchFade(GameObject sourceObject, float fade_time, float pitch_num)
    {
        if (pitch_num < -3)
        {
            pitch_num = -3;
        }
        else if (pitch_num > 3)
        {
           pitch_num = 3;
        }
        float change_time = 0;
        float change_in_pitch = (pitch_num - sourceObject.GetComponent<AudioSource>().pitch) / fade_time;
        while (change_time <= fade_time)
        {
            sourceObject.GetComponent<AudioSource>().pitch += change_in_pitch;
            yield return new WaitForSeconds(1f);
            fade_time++;
        }
        if (pitch_num < -3)
        {
            pitch_num = -3;
        }
        else if (pitch_num > 3)
        {
            pitch_num = 3;
        }
    }
    IEnumerator priority(GameObject sourceObject, float fade_time, float priority_num)
    {
        if (priority_num < 0)
        {
            priority_num = 0;
        }
        else if (priority_num > 256)
        {
            priority_num = 256;
        }
        float change_time = 0;
        int change_in_priority = (int)((priority_num - sourceObject.GetComponent<AudioSource>().priority) / fade_time);
        while (change_time <= fade_time)
        {
            sourceObject.GetComponent<AudioSource>().volume += change_in_priority;
            yield return new WaitForSeconds(0.1f);
            fade_time++;
        }
        if (priority_num < 0)
        {
            priority_num = 0;
        }
        else if (priority_num > 256)
        {
            priority_num = 256;
        }
    }
    //Delete Audio Source
    public void DeleteSource(string source_name)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
        if (cur_m_audio_source != null)
        {
            Sound_Track cur_track = cur_m_audio_source.GetComponent<Audio_Data>().ad_track;
            if (cur_track == null)
            {
                Debug.Log("The current track is null");
                
            }else if(cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Fade_Out)
            {
                StartCoroutine(PitchFade(cur_m_audio_source, cur_track.Fade_Out_Translating, 0));
                StartCoroutine(VolumeFade(cur_m_audio_source, cur_track.Fade_Out_Translating,0));

            }
            else
            {
                cur_m_audio_source.GetComponent<AudioSource>().pitch = 0;
                cur_m_audio_source.GetComponent <AudioSource>().volume = 0;
            }
            m_audio_source_list.Remove(source_name);
            m_sound_name_list.Remove(source_name);
            Destroy(cur_m_audio_source);
        }
    }

    //Check if source is null
    public bool SourceCheck(GameObject obj)
    {
        if (obj == null)
        {
            Debug.Log("Audio Source Object did not clone itself.");
            return true;
        }
        return false;
    }
    //Check if track number is not out of bound
    public bool BoundCheck(int index_num)
    {
        if (index_num > sp_dict.getDictSize() - 1)
        {
            Debug.Log("The track number is out range of dictionary");
            return true;
        }
        return false;
    }
}
