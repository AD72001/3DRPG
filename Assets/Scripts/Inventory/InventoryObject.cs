using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory/Inventory Object")]
public class InventoryObject : ScriptableObject
{
    public string saveLocation = "/Inventory.sav";
    public Inventory container;
    public ItemDatabase database;

    public bool AddItem(Item _item, int _amount)
    {
        InventorySlot slot = ItemInInventory(_item);

        if (!CheckEmptySlot() && !database.Items[_item.Id].stackable)
        {
            return false;
        }

        if (slot == null || !database.Items[_item.Id].stackable)
        {
            SetItemInEmptySlot(_item, _amount);
            return true;
        }

        Debug.Log("Stackable");

        slot.AddAmount(_amount);

        return true;
    }

    public InventorySlot ItemInInventory(Item _item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item.Id == _item.Id)
            {
                return container.items[i];
            }
        }
        return null;
    }

    public bool CheckEmptySlot()
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item.Id <= -1)
            {
                return true;
            }
        }
        return false;
    }

    public InventorySlot SetItemInEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item.Id <= -1)
            {
                container.items[i].UpdateSlot(_item, _amount);
                return container.items[i];
            }
        }

        return null;
    }

    public void SwapItem(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);

            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].item == _item)
            {
                container.items[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, saveLocation));
        bf.Serialize(file, saveData);
        file.Close();

    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, saveLocation)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, saveLocation), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container.Clear();
    }
}

[Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[24];

    public void Clear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].RemoveItem();
        }
    }
}

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedTypes = new ItemType[0];
    [NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;


    public ItemObject ItemObject
    {
        get {
            if (item.Id >= 0)
            {
                return parent.inventory.database.Items[item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        item = new Item();
        amount = 0;
    }

    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(Item _item, int _amount)
    {   
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }

    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (allowedTypes.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
            return true;
        
        for (int i = 0; i < allowedTypes.Length; i++)
        {
            if (_itemObject.type == allowedTypes[i])
                return true;
        }

        return false;
    }
}
