using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool isChoosingSeeds;


    public List<PlantSO> myPlants;
    public List<PlantSO> myPlantBag;


    List<PlantSO> ogBagPos;


    public Transform seedpackets_down, seedpackets_up;

    public Transform bagSlots, mySlots;
    public PlantSO selectedPlant;

    public int sunCount = 0;

    public Transform cursor;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ogBagPos = new List<PlantSO>(myPlantBag);
    }

    public void CheckSuns() {
        sunCount = Mathf.Clamp(sunCount, 0, 9990);
    }

    public void AddSun(int value) {
        sunCount += value;
    }

    public void CursorUpdate() {
        cursor.transform.position = Mouse.current.position.ReadValue();
        Cursor.visible = false;
    }
    void Update()
    {
        CheckSuns();
        CursorUpdate();

        if (Keyboard.current.jKey.wasPressedThisFrame) AddSun(-25);
        if (Keyboard.current.kKey.wasPressedThisFrame) AddSun(25);
    }


    public void SelectSeed(PlantSO plant) {
        if (!isChoosingSeeds) {
            SoundManager.instance.Play("sfx_pickseed");
            return;
        }


        SoundManager.instance.Play("sfx_pickseed_cys");
        bool isInBag = this.myPlantBag.Contains(plant);

        if (isInBag)
        {
            this.myPlantBag.Remove(plant);
            this.myPlants.Add(plant);
        }
        else {
            this.myPlantBag.Add(plant);
            this.myPlants.Remove(plant);
        }
    }


    public void SelectPlant(PlantSO plant) { this.selectedPlant = plant; }


    public Vector2 GetPlantUIPosition(PlantSO plant) { 
        bool isInBag = this.myPlantBag.Contains(plant);


        if (isInBag)
        {
            int index = this.ogBagPos.FindIndex(x => x == plant);
            Vector2 pos = this.bagSlots.GetChild(index).position;


            return pos;
        }
        else {
            int index = this.myPlants.FindIndex(x => x == plant);
            Vector2 pos = this.mySlots.GetChild(index).position;


            return pos;
        }
    }


    public Transform GetPlantUILayering(PlantSO plant)
    {
        return this.myPlantBag.Contains(plant) ? this.seedpackets_down : seedpackets_up;
    }
    public void PrepareGame()
    {
        this.isChoosingSeeds = true;
    }
    public void StartGame() {
        this.isChoosingSeeds = false;
    }

}


