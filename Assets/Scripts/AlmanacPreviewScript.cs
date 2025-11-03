using UnityEngine;

public class AlmanacPreviewScript : MonoBehaviour
{

    public PlantSO plant;
    public bool auto;
    public Transform parentObjectList;
    public void UpdatePreview() {
        if (auto) {
            plant = GameManager.instance.selectedPlant;
        }


        for (int i = 0; i < parentObjectList.childCount; i++) { 
            parentObjectList.GetChild(i).gameObject.SetActive(false);
        }

        parentObjectList.Find(plant.name.ToLower()).gameObject.SetActive(true);
    }
 
    void Update()
    {
        UpdatePreview();
    }
}
