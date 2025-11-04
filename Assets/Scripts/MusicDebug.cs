using TMPro;
using UnityEngine;

public class MusicDebug : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text text2;
    void Start()
    {
        
    }

    void Update()
    {
        string isWave = (MusicManager.instance.playingSectionInfo == PlayingSectionInfo.Normal) ? MusicManager.instance.wave ? "On Wave" : "Normal" : "Final Wave";
        text.text = $"Music Playing ({MusicManager.instance.currentMusic.name}) - Wave-{isWave} - SEC-{MusicManager.instance.currentSection.sectionName} {MusicManager.instance.mainSrc.time} -> {MusicManager.instance.nextSection.time}".ToUpper();
        string icomingWave = LevelManager.instance.incomingWave ? "YES" : "NO";
        string ont = MusicManager.instance.onTransition ? "ON TRANSITION" : "";
        text2.text = $"Zombies on Screen ({LevelManager.instance.onScreenZombies}) - Incoming Wave {icomingWave} {ont}".ToUpper();
        

    }
}
