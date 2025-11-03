using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedPacketUI : MonoBehaviour
{
    public PlantSO plantSO;
    public TMP_Text number;

    public Image icon_a, typeplantSprite;

    public float speed;
    public float z;

    void Update()
    {
        number.text = plantSO.price.ToString();
        icon_a.sprite = plantSO.icon;
        icon_a.SetNativeSize();
        icon_a.rectTransform.localScale = new Vector3(plantSO.iconSize.x, plantSO.iconSize.y, 1f);

        typeplantSprite.sprite = plantSO.typePlantSprite;


        Vector2 target = GameManager.instance.GetPlantUIPosition(this.plantSO);
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target.x, target.y, z), speed*Time.deltaTime);
        this.transform.parent = GameManager.instance.GetPlantUILayering(this.plantSO);

    }

    public void Select() {
        GameManager.instance.SelectSeed(plantSO);

    }

    public void Hover() { 
        GameManager.instance.SelectPlant(plantSO);
    }
}
