using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public void SaveData()
    {
        // Save Inventory data
        GetComponent<CharacterInventory>().equipment.Save();
        GetComponent<CharacterInventory>().inventory.Save();

        // Save Stats
        GetComponent<Stat>().Save();

        // Save Position
        GetComponent<CharacterMovement>().SaveData();

        // Save Map Items states
        ItemManager.instance.SaveData();

        // Save Unique enemy states
        EnemyManager.instance.SaveData();
    }

    public void LoadData()
    {
        // Load Inventory data
        GetComponent<CharacterInventory>().equipment.Load();
        GetComponent<CharacterInventory>().inventory.Load();

        // Load Stats0
        GetComponent<Stat>().Load();

        // Load Position
        GetComponent<CharacterMovement>().LoadData();

        // Load Map Items states
        ItemManager.instance.LoadData();

        // Load Unique enemy states
        EnemyManager.instance.LoadData();
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }
}
