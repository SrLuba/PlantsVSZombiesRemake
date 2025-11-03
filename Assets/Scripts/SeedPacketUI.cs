using TMPro;
using UnityEngine;

public class SeedPacketUI : MonoBehaviour
{
    public PlantSO plantSO;
    public TMP_Text number;



    void Update()
    {
        number.text = plantSO.price.ToString();
    }
}
