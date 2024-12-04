using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject[] itemList;
    public string saveLocation = "/itemManager.sav";
    [SerializeField] private bool[] values;

    public static ItemManager instance {get; private set; }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        itemList = GameObject.FindGameObjectsWithTag("Item");
        values = new bool[itemList.Length];
    }

    public void SaveData()
    {
        values = new bool[itemList.Length];

        int index = 0;

        foreach (GameObject item in itemList)
        {
            values[index] = item.GetComponent<ItemSpawner>().pickedUp;
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

        foreach (GameObject item in itemList)
        {
            item.SetActive(!values[index]);
            index++;
        }
    }
}
