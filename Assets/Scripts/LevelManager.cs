using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    public GameObject introductionUIObj;
    public SmoothTextController houseText;
    public LerpObject SeedUI, introductionUILO;

    public GameObject CYSPanel;

    public TMP_Text readysetgo1, readysetgo2;
    public GameObject readysetgo;
    public Animator rsganim;


    public List<float> sizes;

    public List<JumpingObject> lawnMowers;


    void Start()
    {
        PrepareLevel();
    }


    public void PrepareLevel() {
        MusicManager.instance.Play("cys");
        MusicManager.instance.SetVolume(0f);
        MusicManager.instance.SetVolumeBlending(1f, 1f);

        StartCoroutine(PrepareLevelIE());
    }

    public IEnumerator PrepareLevelIE() {
        SeedUI.Reset();
        GameManager.instance.PrepareGame();

        introductionUIObj.SetActive(true);
        introductionUILO.show = true;

        CameraController.instance.Reset();
        houseText.show = true;
        yield return new WaitForSeconds(2f);

        houseText.show = false;
        yield return new WaitForSeconds(0.5f);

        SeedUI.show = true;
        CameraController.instance.cys = true;
        yield return new WaitForSeconds(0.6f);
        CYSPanel.SetActive(true);
    }


    public void StartLevel() { 
        StartCoroutine(StartLevelIE());
    }

    public IEnumerator StartLevelIE() {
        introductionUILO.show = false;
        CameraController.instance.cys = false;
        GameManager.instance.StartGame();

        yield return new WaitForSeconds(2f);
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

        MusicManager.instance.SetVolume(1f);
        MusicManager.instance.SetVolumeBlending(0f, 1f);


        //READY
        {
            // Ready!
            rsganim.Play("rsg", 0, 0f);

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
            rsganim.Play("rsg", 0, 0f);

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
            rsganim.Play("rsg", 0, 0f);

            // Set Text.
            readysetgo1.text = "PLANT!";
            readysetgo2.text = "PLANT!";

            // Set Size.
            readysetgo1.fontSize = sizes[2];
            readysetgo2.fontSize = sizes[2];


        }


        MusicManager.instance.Play("grasswalk");
        MusicManager.instance.SetVolumeBlending(1f, 1f);
        MusicManager.instance.SetVolume(1f);

    }
}
