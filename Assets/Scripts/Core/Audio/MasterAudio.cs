using System;
using GameFoundation.Scripts.Utilities;
using GameFoundation.Scripts.Utilities.Extension;
using Setting;
using UnityEngine;
using Zenject;

public class MasterAudio : MonoBehaviour
{
    public static MasterAudio Instance;
    private const string WINSOUND = "win_sound";

    public AudioSource musicAudioSource;
    public AudioSource soundAudioSource;

    [Inject] private SettingManager settingManager;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance         = this;
        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        this.GetCurrentContainer().Inject(this);

        settingManager.OnDataLoadedCompleted += SettingManager_OnDataLoaded;
    }

    void SettingManager_OnDataLoaded()
    {
        musicAudioSource.mute = !settingManager.GetMusicState();
        soundAudioSource.mute = !settingManager.GetSoundState();
    }

    public void ToggleMusic()
    {
        settingManager.SetMusicState(musicAudioSource.mute);
        musicAudioSource.mute = !musicAudioSource.mute;
    }

    public void ToggleSound()
    {
        Debug.Log(123);
        settingManager.SetSoundState(soundAudioSource.mute);
        soundAudioSource.mute = !soundAudioSource.mute;
    }

    public void PlayWinSound()
    {
        AudioManager.Instance.PlaySound(WINSOUND, soundAudioSource);
    }

    public void PlaySound(string sound)
    {
        AudioManager.Instance.PlaySound(sound, soundAudioSource);
    }
}