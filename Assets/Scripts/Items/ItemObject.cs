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
    public int buyCost;
    public int sellCost;
    public ItemBonusAttribute[] statBonus;
    public ItemRegenValue[] regenValues;

    public Item()
    {
        name = "";
        Id = -1;
    }

    public Item(ItemObject _itemObject)
    {
        name = _itemObject.name;
        Id = _itemObject.data.Id;

        statBonus = new ItemBonusAttribute[_itemObject.data.statBonus.Length];
        regenValues = new ItemRegenValue[_itemObject.data.regenValues.Length];

        for (int i = 0; i < statBonus.Length; i++)
        {
            statBonus[i] = new ItemBonusAttribute(_itemObject.data.statBonus[i].min, _itemObject.data.statBonus[i].max);
            statBonus[i].attribute = _itemObject.data.statBonus[i].attribute;
        }

        for (int i = 0; i < regenValues.Length; i++)
        {
            regenValues[i] = new ItemRegenValue(_itemObject.data.regenValues[i].value);
            regenValues[i].regenType = _itemObject.data.regenValues[i].regenType;
        }

        buyCost = _itemObject.data.buyCost;
        sellCost = buyCost * 7 / 10;
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