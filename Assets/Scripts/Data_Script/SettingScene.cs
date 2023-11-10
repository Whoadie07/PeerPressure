using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour
{
    public Setting m_Setting;

    public float MusicS  = 100;
    public float VolumeS = 100;
    public float SoundEffectS = 100;
    public Slider MusicSlider;
    public Slider VolumeSlider;
    public Slider SoundEffectSlider;
    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider.value = m_Setting.Volume * 0.01f;
        MusicSlider.value = m_Setting.Music * 0.01f;
        SoundEffectSlider.value = m_Setting.SoundEffect * 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        SetSoundEffect();
        SetMusic();
        SetVolume();
    }
    

    public void SetVolume()
    {
        VolumeS = VolumeSlider.value * 100;
        m_Setting.Volume = VolumeS;
    }
    public void SetMusic()
    {
        MusicS = MusicSlider.value * 100;
        m_Setting.Music = MusicS;
    }
    public void SetSoundEffect()
    {
        SoundEffectS = SoundEffectSlider.value * 100;
        m_Setting.SoundEffect = SoundEffectS;
    }
}
