using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;

    [TextArea(10, 20)]
    public string description;
}
