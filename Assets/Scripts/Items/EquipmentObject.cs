using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Object", menuName = "Inventory/Items/Equipment Item")]
public class EquipmentObject : ItemObject
{
    private void Awake() {
        type = ItemType.Armor;
    }
}
