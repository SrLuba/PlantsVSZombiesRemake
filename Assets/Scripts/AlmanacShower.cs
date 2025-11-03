using TMPro;
using UnityEngine;

public class AlmanacShower : MonoBehaviour
{

    public PlantSO plant;
    public bool auto;

    public TMP_Text plantNameUp, plantName, description, quote, damage, range, cooldown, health;




    public void AutomaticSetPlant() {
        if (!auto) return;

        plant = GameManager.instance.selectedPlant;
    }

    void Update()
    {
        AutomaticSetPlant();
        if (plant == null) return;



        plantNameUp.text = plant.displayName[0].ToString().ToUpper();
        plantName.text = plant.displayName.Substring(1);
        description.text = plant.description;
        quote.text = plant.quote;
        damage.text = $"{plant.damage} - {plant.attackCooldown}s";
        range.text = $"{plant.range.x}x{plant.range.y}";
        cooldown.text = $"{plant.rechargeCooldown}s";
        health.text = plant.health.ToString();
    }
}
