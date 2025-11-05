using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;



public enum LevelAppareanceType { 
    Day,
    Night
}

public static class JumpUtility
{

    public static Vector2 GetJumpPosition(Vector2 start, Vector2 end, float jumpHeight, float t)
    {
        // Interpolación lineal entre los puntos (movimiento horizontal)
        Vector2 horizontal = Vector2.Lerp(start, end, t);

        // Movimiento vertical con forma de arco (usando seno)
        float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;

        // Combina ambas posiciones
        return new Vector2(horizontal.x, horizontal.y + yOffset);
    }
}

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject introductionUIObj;
    public SmoothTextController houseText;
    public LerpObject SeedUI, introductionUILO;

    public GameObject CYSPanel;

    public TMP_Text readysetgo1, readysetgo2;
    public GameObject readysetgo;
    public Animator rsganim;


    public List<float> sizes;

    public List<JumpingObject> lawnMowers;

    public bool incomingWave;
    public bool onWave;

    public int onScreenZombies = 0;

    public SplashScreenScript waveTinyText, waveBigText, finalWaveText;

    public bool finalWave;

    public LevelSO currentLevel;
    GameObject levelGB;

    public void LoadLevel(LevelSO lvl)
    {
        this.currentLevel = lvl;

        if (levelGB != null) {
            Destroy(levelGB);
        }

        GameObject level = currentLevel.levelPrefab;

        levelGB = Instantiate(level);
        levelGB.transform.parent = this.transform.GetChild(0);
        levelGB.transform.localPosition = Vector3.zero;
        UIManager.instance.currentStyle = lvl.uiStyle;
    }

    public void StartWave(bool last) {
        incomingWave = true;
        StartCoroutine(WaveSequence(last));
    }

    public IEnumerator WaveSequence(bool last) {
        yield return new WaitForSeconds(7f);
        SoundManager.instance.Play("hugewave_incoming");
      

        if (last)
        {
            waveBigText.Splash(5f);
            this.finalWave = true;
            this.onWave = true;
            yield return new WaitForSeconds(1f);

            SoundManager.instance.Play("wave_siren");
            yield return MusicManager.instance.AwaitConnection(-MusicManager.instance.transitionDuration);
            MusicManager.instance.ToggleTransition(0, PlayingSectionInfo.GrandWave);
            MusicManager.instance.finalWave = true;
            yield return new WaitForSeconds(MusicManager.instance.finalWaveTransitionDuration);
            SoundManager.instance.Play("final_wave");
            finalWaveText.Splash(5f);
        }
        else
        {
            waveTinyText.Splash(5f);
        }

        if (!last) yield return new WaitForSeconds(3.5f);
        
        if (!last) { 
            this.onWave = true;
        }

        yield return new WaitForSeconds(1.5f);

        if (!last) SoundManager.instance.Play("wave_siren");
  
        incomingWave = false;
        yield return new WaitForSeconds(1f);
        

    }

    public void StopWave() {
        onWave = false;
      

        if (finalWave) { 
            MusicManager.instance.ToggleTransition(1, PlayingSectionInfo.Normal);
        }
    }
    void Start()
    {
        LoadLevel(this.currentLevel);
        PrepareLevel();
    }


    public void PrepareLevel() {

        MusicManager.instance.PlaySimple(this.currentLevel.chooseYourSeedsMusic);

        StartCoroutine(PrepareLevelIE());
    }

    public IEnumerator PrepareLevelIE() {
        SeedUI.Reset();
        GameManager.instance.PrepareGame();

        introductionUIObj.SetActive(true);
        introductionUILO.show = true;
        CameraController.instance.Reset();

        CameraController.instance.cys = true;

        houseText.show = true;
        yield return new WaitForSeconds(2f);

        houseText.show = false;
        yield return new WaitForSeconds(0.5f);

        SeedUI.show = true;
        yield return new WaitForSeconds(0.6f);
        CYSPanel.SetActive(true);
    }

    public void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame) StartWave(Keyboard.current.leftShiftKey.isPressed);
    }

    public void StartLevel() { 
        StartCoroutine(StartLevelIE());
    }

    public IEnumerator StartLevelIE() {
        introductionUILO.show = false;
        CameraController.instance.cys = false;
        GameManager.instance.StartGame();

        yield return new WaitForSeconds(2f);
        MusicManager.instance.SetMainVolumeTarget(0f);

        yield return ShowLawnMowers();

        yield return ReadyStartGo();
    }
    public IEnumerator ShowLawnMowers() {
        for (int i = 0; i < lawnMowers.Count; i++) {
            lawnMowers[i].Reset();
            lawnMowers[i].should = true;

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

    }
    public IEnumerator ReadyStartGo() {
        SoundManager.instance.Play("sfx_readysetplant"); // Play Sound

        //MusicManager.instance.SetVolume(1f);
        //MusicManager.instance.SetVolumeBlending(0f, 1f);


        //READY
        {
            // Ready!
            rsganim.Play("splash", 0, 0f);

            // Set Text.
            readysetgo1.text = "Ready...";
            readysetgo2.text = "Ready...";

            // Set Size.
            readysetgo1.fontSize = sizes[0];
            readysetgo2.fontSize = sizes[0];

        }
        yield return new WaitForSeconds(0.7f);

        //SET
        {
            // Set!
            rsganim.Play("splash", 0, 0f);

            // Set Text.
            readysetgo1.text = "Set...";
            readysetgo2.text = "Set...";

            // Set Size.
            readysetgo1.fontSize = sizes[1];
            readysetgo2.fontSize = sizes[1];


        }
        yield return new WaitForSeconds(0.6f);

        //PLANT!
        {
            // PLANT!
            rsganim.Play("splash", 0, 0f);

            // Set Text.
            readysetgo1.text = "PLANT!";
            readysetgo2.text = "PLANT!";

            // Set Size.
            readysetgo1.fontSize = sizes[2];
            readysetgo2.fontSize = sizes[2];


        }


        MusicManager.instance.PrepareMusic(this.currentLevel.music);
        //MusicManager.instance.SetVolumeBlending(1f, 1f);
        //MusicManager.instance.SetVolume(1f);

    }
}
