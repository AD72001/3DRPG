using System;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Helmet,
    Armor,
    Boots,
    Weapon,
    Gloves,
    Acc,
    Default
}

public enum Attribute
{
    Strength,
    Defense,
    Vitality,
    Intelligent
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items/Item")]
public class ItemObject : ScriptableObject
{
    public Sprite iconDisplay;
    public ItemType type;
    public bool stackable;

    [TextArea(10, 20)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {
        return new Item(this);
    }
}

[Serializable]
public class Item
{
    public string name;
    public int Id = -1;
    public ItemBonus[] statBonus;

    public Item()
    {
        name = "";
        Id = -1;
    }

    public Item(ItemObject _item)
    {
        name = _item.name;
        Id = _item.data.Id;
        statBonus = new ItemBonus[_item.data.statBonus.Length];
        for (int i = 0; i < statBonus.Length; i++)
        {
            statBonus[i] = new ItemBonus(_item.data.statBonus[i].min, _item.data.statBonus[i].max);
            statBonus[i].attribute = _item.data.statBonus[i].attribute;
        }
    }
}

[Serializable]
public class ItemBonus
{
    public Attribute attribute;
    public int value;
    public int min, max;

    public ItemBonus(int _min, int _max)
    {
        min = _min;
        max = _max;
        SetRandomValue();
    }

    public void SetRandomValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}