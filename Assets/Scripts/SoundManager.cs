using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void Play(string clipName) {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + clipName);

        AudioSource src = new GameObject("sfx_"+clip.name).AddComponent<AudioSource>(); 
        src.clip = clip;
        src.loop = false;
        src.Play();
    }
}
