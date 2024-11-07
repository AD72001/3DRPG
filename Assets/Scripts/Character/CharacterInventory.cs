using UnityEngine;

public class CharacterInventory: MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.Load();
            equipment.Load();
        }
    }

}
