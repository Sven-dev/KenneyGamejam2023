using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void OnMasterVolumeChanged(float value)
    {
        AudioManager.Instance.MasterVolume = value;
    }

    public void OnSoundEffectsChanged(float value)
    {
        AudioManager.Instance.SoundVolume = value;
    }

    public void OnMusicChanged(float value)
    {
        AudioManager.Instance.MusicVolume = value;
    }

    public void OnAmbienceChanged(float value)
    {
        AudioManager.Instance.DialogueVolume = value;
    }
}
