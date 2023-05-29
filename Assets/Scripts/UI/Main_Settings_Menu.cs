using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Main_Settings_Menu : MonoBehaviour
{
    public AudioMixer audio_mixer;

    public void SetVolume(float volume)
    {
        audio_mixer.SetFloat("Volume", volume);
    }

    public void SetFullScreen (bool is_fullscreen)
    {
        Screen.fullScreen = is_fullscreen;
    }
}
