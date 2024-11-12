using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        slotDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDragging(obj); });

            inventory.GetSlots[i].slotDisplayed = obj;

            slotDisplayed.Add(obj, inventory.GetSlots[i]);
        }
    }

    public override void OnClick(GameObject obj)
    {        
        Debug.Log("Clicked on " + obj);
        switch (slotDisplayed[obj].ItemObject.type)
        {
            case ItemType.Consumable:
                Debug.Log("Use the item");
                break;
            case ItemType.Default:
                break;
            default:
                Debug.Log("Equip the item");
                inventory.SwapItem(slotDisplayed[obj], inventory.GetSlots[0]);
                break;
        }
    }
}
