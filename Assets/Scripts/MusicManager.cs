using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // will go through a rewrite very soon, this is to get the basic thing working.
    public static MusicManager instance;
    public AudioSource source;

    float currentVolume = 0f;
    public float targetVolume = 0f;
    public float volumeSpeed = 5f;
    private void Awake()
    {
        instance = this;
    }
    public void SetVolume(float volume) { currentVolume = volume; }
    public void UpdateVolume() {

        currentVolume = Mathf.MoveTowards(currentVolume, targetVolume, Time.deltaTime * volumeSpeed);
    }
    public void Update()
    {
        UpdateVolume();
        source.volume = currentVolume;
    }
    public void SetVolumeBlending(float targetVolume, float speed) { 
        this.targetVolume = targetVolume;
        this.volumeSpeed = speed;   
    }
    public void Play(string clipName) {
        AudioClip clip = Resources.Load<AudioClip>("Music/" + clipName);

        source.clip = clip;
        source.Play();
    }
}
