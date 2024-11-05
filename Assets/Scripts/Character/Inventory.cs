using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryObject inventory;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.Load();
        }
    }

}
