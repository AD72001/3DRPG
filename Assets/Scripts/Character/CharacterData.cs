using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterData : MonoBehaviour
{
    private void Start() {
        if (LoadStatus.LoadGame)
            LoadData();
    }

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
Debug.Log(1);
        // Load Stats
        GetComponent<Stat>().Load();
        CheckSkillUnlocked();
Debug.Log(2);
        // Load HP
        GetComponent<HP>().Load();
Debug.Log(3);
        // Load Position
        GetComponent<CharacterMovement>().LoadData();
Debug.Log(4);
        // Load Map Items states
        ItemManager.instance.LoadData();
        ItemFactory.instance.DeactiveAll();
Debug.Log(5);
        // Load Unique enemy states
        EnemyManager.instance.LoadData();
        EnemyFactory.instance.DeactiveAll();
        Debug.Log(6);
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
