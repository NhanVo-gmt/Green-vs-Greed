using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    HitImpact,
    GetShield,
    UseShield,
    LoseShield,
    DrawCard,
    PickCard,
    RightAnswer,
    WrongAnswer,
    Reward,
}

[Serializable]
public class SoundData
{
    public SoundType SoundType;
    public AudioClip Clip;
    public float     Volume = 1f;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<SoundData> SoundDatas = new();
    [SerializeField] private AudioSource     Source;

    private Dictionary<SoundType, SoundData> SoundDataDict = new();

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach (var soundData in SoundDatas)
        {
            SoundDataDict.TryAdd(soundData.SoundType, soundData);
        }
    }

    public void PlayOneShot(SoundType type)
    {
        Source.PlayOneShot(SoundDataDict[type].Clip, SoundDataDict[type].Volume);
    }
}
