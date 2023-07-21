
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditorInternal;
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

    //Stop the Audio Source
    public void StopAudioSource(string source_name)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
        if (cur_m_audio_source != null)
        {
            if (!(cur_m_audio_source.GetComponent<AudioSource>().isPlaying))
            {
                Debug.Log("The Audio Source is current not play");
                return;
            }
            if (cur_m_audio_source.GetComponent<Audio_Data>().ad_track.StopSource)
            {
                StartCoroutine(StopFade(cur_m_audio_source, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Stop_time, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Stop_time_vol));
            }
            else
            {
                cur_m_audio_source.GetComponent<AudioSource>().Stop();
            }
        }
        else
        {
            Debug.Log("Audio Source with name don't exist.");
        }
    }
    //Pause the Audio Source
    public void PauseAudioSource(string source_name)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
        if (cur_m_audio_source != null)
        {
            if (cur_m_audio_source.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Audio Source is current not play");
                return;
            }
            if (cur_m_audio_source.GetComponent<Audio_Data>().ad_track.PauseSource)
            {
                StartCoroutine(PauseFade(cur_m_audio_source, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Pause_time, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Pause_time_vol));
            }
            else
            {
                cur_m_audio_source.GetComponent<AudioSource>().Pause();
            }
        }
        else
        {
            Debug.Log("Audio Source with name don't exist.");
        }
    }
    //Resume the Audio Source 
    public void ResumeAudioSource(string source_name)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
        if (cur_m_audio_source != null)
        {
            if (cur_m_audio_source.GetComponent<AudioSource>().isPlaying)
            {
                Debug.Log("Audio Source is currently play");
                return;
            }
            if (cur_m_audio_source.GetComponent<Audio_Data>().ad_track.ResumeSource)
            {
                StartCoroutine(ResumeFade(cur_m_audio_source, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Resume_time, cur_m_audio_source.GetComponent<Audio_Data>().ad_track.Resume_time_vol));
            }
            else
            {
                cur_m_audio_source.GetComponent<AudioSource>().Pause();
            }
        }
        else
        {
            Debug.Log("Audio Source with name don't exist.");
        }
    }

    //Change the track on Audio Source
    public void ChangeTrack(string source_name, int track_num)
    {
        StartCoroutine(RealChangeTrack(source_name, track_num));
    }
    //Real Change 
    IEnumerator RealChangeTrack(string source_name, int track_num)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_name] as GameObject;
        if (cur_m_audio_source != null)
        {
            if (cur_m_audio_source.GetComponent<AudioSource>().isPlaying)
            {
                if (m_audio_source.GetComponent<Audio_Data>().ad_track.Fade_Out)
                {
                    StartCoroutine(VolumeFade(cur_m_audio_source, m_audio_source.GetComponent<Audio_Data>().ad_track.Fade_Out_Translating, 0));
                    yield return new WaitForSeconds(m_audio_source.GetComponent<Audio_Data>().ad_track.Fade_Out_Translating);
                }
                else
                {
                    cur_m_audio_source.GetComponent<AudioSource>().volume = 0;
                }
                cur_m_audio_source.GetComponent<Audio_Data>().ad_track = sp_dict.tracks[track_num];
                PlayAudioSource(source_name);
            }
            else
            {
                cur_m_audio_source.GetComponent<Audio_Data>().ad_track = sp_dict.tracks[track_num];
                PlayAudioSource(source_name);
            }
        }
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
    IEnumerator StopFade(GameObject sourceObject, float fade_time, float volume_num)
    {
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
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
        sourceObject.GetComponent<AudioSource>().Stop();
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
        {
            volume_num = 1;
        }
    }
    IEnumerator PauseFade(GameObject sourceObject, float fade_time, float volume_num)
    {
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
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
        sourceObject.GetComponent<AudioSource>().Stop();
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
        {
            volume_num = 1;
        }
    }
    IEnumerator ResumeFade(GameObject sourceObject, float fade_time, float volume_num)
    {
        if (volume_num < 0)
        {
            volume_num = 0;
        }
        else if (volume_num > 1)
        {
            volume_num = 1;
        }
        sourceObject.GetComponent<AudioSource>().UnPause();
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
        else
        {
            Debug.Log("Audio Source with name don't exist.");
        }
    }

    //Clear all the Audio Source for hashtable
    public void ClearAllSources()
    {
        for (int i = 0; i < m_sound_name_list.ToArray().Length; i++)
        {
            DeleteSource((string) m_sound_name_list.ToArray().GetValue(i));
        }
        m_audio_source_list.Clear();
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

    //volume: (0 - 100)
    //Pitch: (-3 - 100)
    //Priority (0 - 256)
    //Change the Proporites of Audio Source
    public void SetSourceVolume(string source_num, float volume)
    {
        if(volume > 0) { volume  = 0; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().volume = (volume / 100); }
    }
    public void SetSourcePriority(string source_num, int priority)
    {
        if (priority < 0) { priority = 0; } else if (priority > 256) { priority = 256; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().priority = priority; }
    }
    public void SetSourcePitch(string source_num, float pitch)
    {
        if (pitch < 0) { pitch = 0; } else if (pitch > 3) { pitch = 3; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().pitch = pitch; }
    }
    public void SetSourceLoop(string source_num, bool Changed)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().loop = Changed; }
    }
    public void SetSourceMute(string source_num, bool Changed)
    {
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().mute = Changed; }
    }
    public void SetSourceStereoPan(string source_num, float st_pan)
    {
        if (st_pan < -1) { st_pan = -1; } else if (st_pan > 1) { st_pan = 1; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().panStereo = st_pan; }
    }
    public void SetSourceSpatialBlend(string source_num, float sp_blend)
    {
        if (sp_blend < -1) { sp_blend = -1; } else if (sp_blend > 1) { sp_blend = 1; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().panStereo = sp_blend; }
    }
    public void SetSourceZoneMix(string source_num, float Zone_Mix)
    {
        if (Zone_Mix < 0) { Zone_Mix = 0.001f; } else if (Zone_Mix > 1.1) { Zone_Mix = 1.1f; }
        GameObject cur_m_audio_source = m_audio_source_list[source_num] as GameObject;
        if (cur_m_audio_source != null) { cur_m_audio_source.GetComponent<AudioSource>().reverbZoneMix = Zone_Mix; }
    }

    //Change the Proporties of the Track 
    public void SetVolume(int index_num, float volume)
    {
        if(volume < 0)
        {
            volume = 0;
        }
        sp_dict.tracks[index_num].volume = (volume/100);
    }
    public void SetPitch(int index_num, float pitch)
    {
        if (pitch < -3)
        {
            pitch = -3;
        }
        else if (pitch > 3)
        {
            pitch = 3;  
        }
        sp_dict.tracks[index_num].pitch = pitch;
    }
    public void SetPriority(int index_num, int priority)
    {
        if (priority < 0) { priority = 0; } else if (priority > 256) { priority = 256; }
        sp_dict.tracks[index_num].priority = priority;
    }
    
    //Fading Properites Change:
    public void SetFadeIn(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].Fade_In = changed;
    }
    public void SetFadeOut(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].Fade_Out = changed;
    }
    public void SetFadeInTranslate(int index_num, float num)
    {
        if(num < 0) { num = 0.001f;}
        sp_dict.tracks[index_num].Fade_In_Translating = num;
    }
    public void SetFadeOutTranslate(int index_num, float num)
    {
        if (num < 0) {num = 0.001f;}
        sp_dict.tracks[index_num].Fade_Out_Translating = num;
    }

    //Stop, Pause, and Resume Proerites Change:
    public void SetStopSource(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].StopSource = changed;
    }
    public void SetPauseSource(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].PauseSource = changed;
    }
    public void SetResumeSource(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].ResumeSource = changed;
    }
    public void SetStopTime(int index_num, float num)
    {
        if (num < 0){ num = 0.001f; }
        sp_dict.tracks[index_num].Stop_time = num;
    }
    public void SetPauseTime(int index_num, float num)
    {
        if (num < 0) { num = 0.001f; }
        sp_dict.tracks[index_num].Pause_time = num;
    }
    public void SetResumeTime(int index_num, float num)
    {
        if (num < 0) { num = 0.001f; }
        sp_dict.tracks[index_num].Resume_time = num;
    }
    public void SetStopTimeVolume(int index_num, float num)
    {
        if (num < 0) { num = 0; }
        sp_dict.tracks[index_num].Stop_time_vol = num;
    }
    public void SetPauseTimeVolume(int index_num, float num)
    {
        if (num < 0) { num = 0; }
        sp_dict.tracks[index_num].Pause_time_vol = num;
    }
    public void SetResumeTimeVolume(int index_num, float num)
    {
        if (num < 0) { num = 0; }
        sp_dict.tracks[index_num].Resume_time_vol = num;
    }

    //Chnage the loop proporites
    public void SetLoop(int index_num, bool changed)
    {
        sp_dict.tracks[index_num].looped = changed;
    }
}
