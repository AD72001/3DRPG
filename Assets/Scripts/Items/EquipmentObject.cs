using UnityEngine;

[CreateAssetMenu(fileName = "Equipment Object", menuName = "Inventory/Items/Equipment Item")]
public class EquipmentObject : ItemObject
{
    public int str;
    public int def;
    public int vit;

    private void Awake() {
        type = ItemType.Equipment;
    }
}
