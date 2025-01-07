using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public DynamicInterface inventoryUI;

    [SerializeField] private AudioClip equipItemSound;

    public override void CreateSlots()
    {
        slotDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < thisInventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            // AddEvent(obj, EventTriggerType.PointerClick, delegate {OnClick(obj); });
            AddEvent(obj, EventTriggerType.PointerEnter, delegate{ OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate{ OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate{ OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate{ OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate{ OnDragging(obj); });

            thisInventory.GetSlots[i].slotDisplayed = obj;

            slotDisplayed.Add(obj, thisInventory.GetSlots[i]);
        }
    }

    public override void OnClick(GameObject obj)
    {        
        if (slotDisplayed[obj].ItemObject == null) return;
        DeEquipItem(obj);
    }

    public void DeEquipItem(GameObject obj)
    {
        AudioManager.instance.PlaySound(equipItemSound);

        for (int i = 0; i < inventoryUI.thisInventory.GetSlots.Length; i++)
        {
            if (inventoryUI.thisInventory.GetSlots[i].ItemObject == null) 
            {
                thisInventory.SwapItem(slotDisplayed[obj], inventoryUI.thisInventory.GetSlots[i]);
                return;
            }
        }
    }
}
