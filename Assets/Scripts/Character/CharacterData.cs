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

        // Save HP
        GetComponent<HP>().Save();

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

        // Load Stats
        GetComponent<Stat>().Load();
        CheckSkillUnlocked();

        // Load HP
        GetComponent<HP>().Load();

        // Load Position
        GetComponent<CharacterMovement>().LoadData();

        // Load Map Items states
        ItemManager.instance.LoadData();

        // Load Unique enemy states
        EnemyManager.instance.LoadData();
        EnemyFactory.instance.DeactiveAll();
    }

    private void CheckSkillUnlocked()
    {
        int level = GetComponent<Stat>().level;

        if (level >= 2)
            GetComponent<Skill_2>().enabled = true;
        if (level >= 3)
            GetComponent<Skill_3>().enabled = true;
        if (level >= 6)
            GetComponent<Skill_4>().enabled = true;
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }
}
