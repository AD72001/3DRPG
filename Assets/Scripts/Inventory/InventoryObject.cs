using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum InventoryType
{
    Inventory,
    Equipment,
    Merchant,
    Storage
}

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory/Inventory Object")]
public class InventoryObject : ScriptableObject
{
    public string saveLocation = "/Inventory.sav";
    public Inventory container;
    [SerializeField] public ItemDatabase database;
    public InventoryType type;
    public InventorySlot[] GetSlots {
        get {
            return container.slots;
        }
    }

    private void Awake() {
        database = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    private void OnEnable() {
        database = Resources.Load<ItemDatabase>("ItemDatabase");
    }

    public bool AddItem(Item _item, int _amount)
    {
        InventorySlot slot = ItemInInventory(_item);

        if (!CheckEmptySlot() && !database.ItemObjects[_item.Id].stackable)
        {
            return false;
        }

        if (slot == null || !database.ItemObjects[_item.Id].stackable)
        {
            SetItemInEmptySlot(_item, _amount);
            return true;
        }

        slot.AddAmount(_amount);

        return true;
    }

    public InventorySlot ItemInInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public bool CheckEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                return true;
            }
        }
        return false;
    }

    public InventorySlot SetItemInEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
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
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot(null, 0);
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

            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].AddAmount(0);
            }

            file.Close();
        }

        database = Resources.Load<ItemDatabase>("ItemDatabase");
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
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot slot);

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedTypes = new ItemType[0];
    [NonSerialized]
    public UserInterface parent;
    [NonSerialized]
    public GameObject slotDisplayed;
    [NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    [NonSerialized]
    public SlotUpdated OnAfterUpdate;

    public Item item = new Item();
    public int amount;

    public ItemObject ItemObject
    {
        get {
            if (item.Id >= 0)
            {
                return parent.thisInventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }

    public void UpdateSlot(Item _item, int _amount)
    {   
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }
        item = _item;
        amount = _amount;
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount + value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
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
