using UnityEngine;
[CreateAssetMenu]
public class PlantSO : ScriptableObject
{
    public int price = 50;
    public string displayName;
    [TextArea] public string description;
    [TextArea] public string quote;


    public int damage = 20;
    public Vector2 range = new Vector2(9,1);
    public int health = 300;
    public float attackCooldown = 1.5f;
    public float rechargeCooldown = 7.5f;
}
