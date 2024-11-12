using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Items/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.Id = i;
        }
    }

    public void OnBeforeSerialize() {}
}
