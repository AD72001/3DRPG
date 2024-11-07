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

    public override void CreateSlots() 
    {
        slotDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDragging(obj); });

            slotDisplayed.Add(obj, inventory.container.items[i]);
        }
    }

    private Vector3 GetPosition(int index)
    {
        return new Vector3(
            x_start + (x_space_between_items * (index % columnNumber)), 
            y_start + (-y_space_between_items * (index / columnNumber)), 0);
    }
}
