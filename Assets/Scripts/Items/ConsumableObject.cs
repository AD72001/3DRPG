using UnityEngine;

[CreateAssetMenu(fileName = "Consumable Object", menuName = "Inventory/Items/Consumable Item")]
public class ConsumableObject : ItemObject
{
    public float HP_value;
    public float MP_value;

    private void Awake() {
        type = ItemType.Consumable;
    }
}
