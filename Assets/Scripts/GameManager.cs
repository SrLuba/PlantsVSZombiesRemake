using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public PlantSO selectedPlant;

    public int sunCount = 0;
    private void Awake()
    {
        instance = this;
    }

    public void CheckSuns() {
        sunCount = Mathf.Clamp(sunCount, 0, 9990);
    }

    public void AddSun(int value) {
        sunCount += value;
    }

    void Update()
    {
        CheckSuns();

        if (Keyboard.current.jKey.wasPressedThisFrame) AddSun(-25);
        if (Keyboard.current.kKey.wasPressedThisFrame) AddSun(25);
    }
}
