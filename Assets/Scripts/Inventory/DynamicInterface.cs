using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public int x_start;
    public int y_start;

    public int x_space_between_items;
    public int y_space_between_items;
    public int columnNumber;

    public StaticInterface equipmentUI;

    public override void CreateSlots() 
    {
        slotDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerClick, delegate{ OnClick(obj); });
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
        for (int i = 0; i < equipmentUI.inventory.GetSlots.Length; i++)
        {
            InventorySlot tempSlot = AvailableSlotSameType(slotDisplayed[obj].ItemObject);
            if (tempSlot != null)
            {
                inventory.SwapItem(slotDisplayed[obj], tempSlot);
                break;
            }
            else if (equipmentUI.inventory.GetSlots[i].CanPlaceInSlot(slotDisplayed[obj].ItemObject))
            {
                inventory.SwapItem(slotDisplayed[obj], equipmentUI.inventory.GetSlots[i]);
                break;
            }
        }
    }

    public InventorySlot AvailableSlotSameType(ItemObject _itemObject)
    {
        for (int i = 0; i < equipmentUI.inventory.GetSlots.Length; i++)
        {
            if (equipmentUI.inventory.GetSlots[i].CanPlaceInSlot(_itemObject) 
            && equipmentUI.inventory.GetSlots[i].ItemObject == null)
            {
                return equipmentUI.inventory.GetSlots[i];
            }
        }
        return null;
    }

    private Vector3 GetPosition(int index)
    {
        return new Vector3(
            x_start + (x_space_between_items * (index % columnNumber)), 
            y_start + (-y_space_between_items * (index / columnNumber)), 0);
    }
}
