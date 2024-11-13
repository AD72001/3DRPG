using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public DynamicInterface inventoryUI;

    public override void CreateSlots()
    {
        slotDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            // AddEvent(obj, EventTriggerType.PointerClick, delegate {OnClick(obj); });
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
        if (slotDisplayed[obj].ItemObject == null) return;
        DeEquipItem(obj);
    }

    public void DeEquipItem(GameObject obj)
    {
        for (int i = 0; i < inventoryUI.inventory.GetSlots.Length; i++)
        {
            if (inventoryUI.inventory.GetSlots[i].ItemObject == null) 
            {
                inventory.SwapItem(slotDisplayed[obj], inventoryUI.inventory.GetSlots[i]);
                return;
            }
        }
    }
}
