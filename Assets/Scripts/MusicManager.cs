using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum PlayingSectionInfo { 
    Normal,
    GrandWave,
    End
}
[System.Serializable]
public enum MusicMode { 
    StandBy,
    Simple,
    Complex
}
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public MusicMode mode;

    public float main_targetVolume;
    public float wave_targetVolume;
    public List<float> sub_targetvolumes;

    public AudioSource mainSrc, waveSrc;
    List<AudioSource> subSrcs;

    public MusicSO currentMusic;

    public int targetSection = 0;

    public bool wave;
    public float waveBlendingTime;

    public float SubSectionBlendingTime = 0f;

    public MusicSection currentSection;
    public MusicSection nextSection;
    public bool loopSection = false;

    public PlayingSectionInfo playingSectionInfo = PlayingSectionInfo.Normal;

    public Vector2Int sectionRange;
    public int ind = 0;

    public float transitionDuration;
    public float finalWaveTransitionDuration;
    public string transitionFileName;
    public string transitionGrandFileName;

    float transitionTimer;
    public bool onTransition;

    public float nextTime;
    public PlayingSectionInfo targetInfo = PlayingSectionInfo.Normal;


    public bool finalWave;
    public Transform subSParen;

    public float simpleVolumeSpeed = 5f;

    public void PlaySimple(MusicSO music) {
        if (this.mode == MusicMode.Complex) {
            UnloadMusic();
        }

        this.currentMusic = music;
        this.mode = MusicMode.Simple;

        this.mainSrc = new GameObject("mus_" + music.name).AddComponent<AudioSource>();
        this.mainSrc.clip = music.mainMusicClip;
        this.mainSrc.volume = 1f;
        this.mainSrc.loop = true;
        this.mainSrc.Play();
        this.main_targetVolume = 1f;
    }

    public void SetMainVolumeTarget(float vol) {
        this.main_targetVolume = vol;
    }

    public void UnloadMusic() {
        if (this.mode != MusicMode.Complex) return;

        if (mainSrc != null) Destroy(mainSrc.gameObject);
        if (waveSrc != null) Destroy(waveSrc.gameObject);

        if (subSrcs != null)
        {
            for (int i = 0; i < subSrcs.Count; i++)
            {
                Destroy(subSrcs[i].gameObject);
            }
        }
        if (subSParen != null) Destroy(subSParen.gameObject);

    }
    float GetNextClosest(List<float> list, float target)
    {
        // Filtramos solo los números >= target
        var mayores = list.Where(n => n >= target);

        // Si hay al menos uno, devolvemos el menor entre ellos (el más cercano hacia arriba)
        if (mayores.Any())
            return mayores.Min();

        // Si no hay ninguno mayor, devolvemos el más grande (opcional)
        return list.Max();
    }

    public IEnumerator AwaitConnection(float offset) {

        float nT = GetNextClosest(this.currentMusic.grandWaveCheckpoints, this.mainSrc.time);
        float waitTime = Mathf.Abs(this.mainSrc.time - nT);

        float addedTime = 0;

        int index = this.currentMusic.grandWaveCheckpoints.FindIndex(x => x == nT);

        if (index == this.currentMusic.grandWaveCheckpoints.Count - 1) {
            addedTime = this.currentMusic.mainSection[this.currentSection.nextRedirect].writtenDuration;
        }


        yield return new WaitForSeconds(waitTime + offset + addedTime);
    }
    public void ToggleTransition(int targetSection, PlayingSectionInfo info) {
        if (onTransition) return;


        SoundManager.instance.Play((info == PlayingSectionInfo.GrandWave) ? transitionGrandFileName : transitionFileName);
        transitionTimer = 0f;
        onTransition = true;

        targetInfo = info;
        this.targetSection = targetSection;
    }

    public void UpdateSub() {
        bool anyOn = false;
        for (int i = 0; i < sub_targetvolumes.Count; i++) {
            if (this.sub_targetvolumes[i] > 0f) anyOn = true;
            if (this.currentMusic.subSections[i].setOnWhenAny) {
                float value = 0f;

                
                for (int a = 0; a < sub_targetvolumes.Count; a++) {
                    if (a != i) value += sub_targetvolumes[a];
                }
                if (!anyOn)
                {
                    value = 0f;
                }
                this.sub_targetvolumes[i] = value;
            }



            if (!this.currentMusic.subSections[i].setOnWhenAny) {
                if (this.currentMusic.subSections[i].effect == MusicDynEffect.ZombieCount)
                {
                    int zombieCount = LevelManager.instance.onScreenZombies;

                    this.sub_targetvolumes[i] = Mathf.Clamp(zombieCount * this.currentMusic.subSections[i].effectiveness, 0f, 1f);


                }
                else if (this.currentMusic.subSections[i].effect == MusicDynEffect.IncomingWave)
                {
                    this.sub_targetvolumes[i] = LevelManager.instance.incomingWave ? 1f : 0f;

                }
            
            }
        }




    }

    public void UpdateTransition()
    {
        if (!onTransition) {
            transitionTimer = 0f;
            return;
        }
        if (transitionTimer < transitionDuration) {
            transitionTimer+= Time.deltaTime;
            return;
        }


        if (wave && !finalWave) {
            LevelManager.instance.onWave = false;
            waveSrc.volume = 0f;
            mainSrc.volume = 1f;
        }
        this.playingSectionInfo = targetInfo;
        this.ForcePlaySection(targetSection);
        onTransition = false;
    }
    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        this.wave = LevelManager.instance.onWave;


        if (this.mode == MusicMode.Simple) {
            mainSrc.volume = Mathf.MoveTowards(mainSrc.volume, main_targetVolume, simpleVolumeSpeed * Time.deltaTime);
            return;
        }

        UpdateVolume();
        UpdateTransition();
        UpdateSub();

        if (Keyboard.current.pKey.wasPressedThisFrame) ForcePlaySection(targetSection);
        if (Keyboard.current.uKey.wasPressedThisFrame) ToggleTransition(0, PlayingSectionInfo.GrandWave);
        if (Keyboard.current.yKey.wasPressedThisFrame) ToggleTransition(0, PlayingSectionInfo.End);
        if (Keyboard.current.wKey.wasPressedThisFrame) SoundManager.instance.Play(this.currentMusic.winJingle);


        if (this.currentSection.nextRedirect >= 0)
        {
            this.nextSection = (this.playingSectionInfo == PlayingSectionInfo.Normal) ? this.currentMusic.mainSection[this.currentSection.nextRedirect] : this.currentMusic.GrandWaveSections[this.currentSection.nextRedirect];
        }
        else
        {
            this.nextSection = null;
        }

        nextTime = this.currentSection.duration < 0f ? this.nextSection.time : (this.currentSection.duration + this.currentSection.time);

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            this.mainSrc.time = nextTime - 1f;
        }

        if (this.mainSrc.time >= nextTime)
        {

            if (finalWave && this.currentSection == this.currentMusic.mainSection[this.currentMusic.GrandPreparation]) {
                ForcePlaySection(this.currentMusic.GrandWaveSections[0]);
                this.playingSectionInfo = PlayingSectionInfo.GrandWave;
                return;
            }

            if (this.currentSection.nextRedirect < 0)
            {

                ForcePlaySection(this.currentSection);

            }
            else
            {
                ForcePlaySection(nextSection);

            }
        }
    }

    public void PrepareMusic(MusicSO music) {
        if (mainSrc != null) Destroy(mainSrc.gameObject);
        if (waveSrc != null) Destroy(waveSrc.gameObject);

        if (subSrcs != null) {
            for (int i = 0; i < subSrcs.Count; i++) { 
                Destroy(subSrcs[i].gameObject);
            }
        }

        this.currentMusic = music;
        this.mode = MusicMode.Complex;

        SetupMusic();
    }

    public void SetupMusic() {
        MusicSO music = this.currentMusic;
        subSrcs = new List<AudioSource>();

        mainSrc = new GameObject(music.name).AddComponent<AudioSource>();
        waveSrc = new GameObject(music.name + "_wav").AddComponent<AudioSource>();


        mainSrc.transform.parent = this.transform;
        waveSrc.transform.parent = this.transform;

        if (subSParen == null) { 
            subSParen = new GameObject("subsections").transform;
            subSParen.parent = this.transform;
        }

        for (int i = 0; i < music.subSections.Count; i++) { 
            AudioSource nAd = new GameObject(music.name + "subsection_"+i.ToString()).AddComponent<AudioSource>();
            nAd.transform.parent = subSParen;


            nAd.clip = music.subSections[i].clip;
            nAd.loop = false;
            nAd.Play();
            nAd.volume = 0f;
            subSrcs.Add(nAd);
        }

        mainSrc.clip = music.mainMusicClip;
        waveSrc.clip = music.mainMusicClip;

        mainSrc.loop = false;
        waveSrc.loop = false;


        mainSrc.Play();
        waveSrc.Play();

        waveSrc.volume = 0f;


    }


    public void UpdateVolume() {
        wave_targetVolume = wave ? 1f : 0f;
        main_targetVolume = !wave ? 1f : 0f;
        mainSrc.volume = Mathf.MoveTowards(mainSrc.volume, main_targetVolume, waveBlendingTime * Time.deltaTime);
        waveSrc.volume = Mathf.MoveTowards(waveSrc.volume, wave_targetVolume, waveBlendingTime * Time.deltaTime);

        for (int i = 0; i < currentMusic.subSections.Count; i++) {
            subSrcs[i].volume = Mathf.MoveTowards(subSrcs[i].volume, sub_targetvolumes[i], SubSectionBlendingTime * Time.deltaTime);
        }
    }
    public void ForcePlaySection(int ie)
    {
        MusicSection sec = (this.playingSectionInfo == PlayingSectionInfo.Normal) ? currentMusic.mainSection[ie] : currentMusic.GrandWaveSections[ie];
        MusicSection rSection = sec;

        if (wave && sec.redirectWave >= 0)
        {
            rSection = (this.playingSectionInfo == PlayingSectionInfo.Normal) ? currentMusic.mainSection[rSection.redirectWave] : currentMusic.GrandWaveSections[rSection.redirectWave];
        }
        if (this.playingSectionInfo == PlayingSectionInfo.End)
        {
            rSection = this.currentMusic.theEnd;
        }
        mainSrc.time = rSection.time;
        waveSrc.time = rSection.qTime;

        for (int i = 0; i < currentMusic.subSections.Count; i++)
        {
            subSrcs[i].time = rSection.time;
        }
        this.currentSection = rSection;

    }
    public void ForcePlaySection(MusicSection sec) {

        MusicSection rSection = sec;

        
        if (wave && sec.redirectWave >=0) { 
            rSection = (this.playingSectionInfo == PlayingSectionInfo.Normal) ? currentMusic.mainSection[sec.redirectWave] : currentMusic.GrandWaveSections[sec.redirectWave];
        }

        if (this.playingSectionInfo == PlayingSectionInfo.End)
        {
            rSection = this.currentMusic.theEnd;
        }
        mainSrc.time = rSection.time;
        waveSrc.time = rSection.qTime;

        for (int i = 0; i < currentMusic.subSections.Count; i++)
        {
            subSrcs[i].time = rSection.time;
        }

        this.currentSection = rSection;
    }

}
