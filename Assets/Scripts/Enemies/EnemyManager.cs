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
        enemyList = GameObject.FindGameObjectsWithTag("EnemySpawner");
        values = new bool[enemyList.Length];
    }

    public void SaveData()
    {
        values = new bool[enemyList.Length];

        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
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

        int index = 0;

        foreach (GameObject enemy in enemyList)
        {
            Debug.Log($"Load: {values[index]}");
            enemy.SetActive(!values[index]);
            index++;
        }
    }
}
