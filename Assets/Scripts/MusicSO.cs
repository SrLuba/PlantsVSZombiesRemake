using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MusicDynEffect { 
    ZombieCount,
    IncomingWave,
    Nothing
}


[System.Serializable]
public class MusicDynSection {
    public string subSectionName;
    public AudioClip clip;
    public float volume;

    public bool setOnWhenAny;
    public float effectiveness;
    public MusicDynEffect effect;

}

[System.Serializable]
public class MusicSection {
    public string sectionName;
    public float time;
    public float qTime;
    public int nextRedirect;
    public float duration;
    public int redirectWave;

    public float writtenDuration;
}

[CreateAssetMenu]
public class MusicSO : ScriptableObject
{
    public AudioClip mainMusicClip;
    public float volume;

    public List<MusicSection> mainSection, GrandWaveSections;
    public MusicSection theEnd;

    public List<MusicDynSection> subSections;
    public int GrandPreparation;


    public string winJingle;

    public List<float> grandWaveCheckpoints;
}
