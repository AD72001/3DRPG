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

public enum Attributes
{
    Strength,
    Defense,
    Vitality,
    Intelligent
}

public enum RegenValues
{
    Health,
    Energy
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
    public ItemBonusAttribute[] statBonus;
    public ItemRegenValue[] regenValues;

    public Item()
    {
        name = "";
        Id = -1;
    }

    public Item(ItemObject _item)
    {
        name = _item.name;
        Id = _item.data.Id;

        statBonus = new ItemBonusAttribute[_item.data.statBonus.Length];
        regenValues = new ItemRegenValue[_item.data.regenValues.Length];

        for (int i = 0; i < statBonus.Length; i++)
        {
            statBonus[i] = new ItemBonusAttribute(_item.data.statBonus[i].min, _item.data.statBonus[i].max);
            statBonus[i].attribute = _item.data.statBonus[i].attribute;
        }

        for (int i = 0; i < regenValues.Length; i++)
        {
            regenValues[i] = new ItemRegenValue(_item.data.regenValues[i].value);
            regenValues[i].regenType = _item.data.regenValues[i].regenType;
        }
    }
}

[Serializable]
public class ItemBonusAttribute: IModifier
{
    public Attributes attribute;
    public int value;
    public int min, max;

    public ItemBonusAttribute(int _min, int _max)
    {
        min = _min;
        max = _max;
        SetRandomValue();
    }

    public void AddValue(ref int _value)
    {
        _value += value;
    }

    public void SetRandomValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}

[Serializable]
public class ItemRegenValue
{
    public RegenValues regenType;
    public int value;

    public ItemRegenValue(int _value)
    {
        value = _value;
    }
}