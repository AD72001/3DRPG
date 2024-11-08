using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Items/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].data.Id = i;
        }
    }

    public void OnBeforeSerialize() {}
}
