using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyList;
    public string saveLocation = "/enemyManager.sav";
    [SerializeField] private bool[] values;

    public static EnemyManager instance {get; private set; }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        GetEnemyList();
        values = new bool[enemyList.Length];
    }

    private void GetEnemyList()
    {
        int l = GameObject.FindGameObjectsWithTag("EnemySpawner").Length;
        enemyList = new GameObject[l];

        string nameByIndex;

        for (int index = 0; index < l; index++)
        {
            if (index < 10)
                nameByIndex = "MonsterSpawner_0" + index.ToString();
            else
                nameByIndex = "MonsterSpawner_" + index.ToString();

            enemyList[index] = GameObject.Find(nameByIndex);
        }
    }

    public void SaveData()
    {
        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
            if (values[index] == true) continue;
            values[index] = enemy.GetComponent<EnemySpawner>().ReActiveCondition();
            index++;
        }

        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, saveLocation));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, saveLocation)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, saveLocation), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }

        GetEnemyList();
        
        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<EnemySpawner>().SetActiveStatus(!values[index]);
            index++;
        }
    }
}
