using UnityEngine;

[CreateAssetMenu(fileName = "Default Object", menuName = "Inventory/Items/Default Item")]
public class DefaultObject : ItemObject
{
    private void Awake() {
        stackable = true;
        type = ItemType.Default;
    }
}
