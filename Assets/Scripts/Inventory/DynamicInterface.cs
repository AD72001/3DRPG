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

        for (int i = 0; i < thisInventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            // AddEvent(obj, EventTriggerType.PointerClick, delegate{ OnClick(obj); });
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

        if (MerchantInterface.isShopping)
        {
            SellItem(obj);
            return;
        }

        switch (slotDisplayed[obj].ItemObject.type)
        {
            case ItemType.Consumable:
                UseItem(obj);
                break;
            case ItemType.Default:
                break;
            default:
                EquipItem(obj);
                break;
        }
    }

    public void SellItem(GameObject obj)
    {
        slotDisplayed[obj].AddAmount(-1);
        player.GetComponent<CharacterInventory>().AddCash(slotDisplayed[obj].item.sellCost);

        if (slotDisplayed[obj].amount <= 0)
        {
            slotDisplayed[obj].RemoveItem();
        }
    }

    public void UseItem(GameObject obj)
    {
        for (int i = 0; i < slotDisplayed[obj].item.regenValues.Length; i++)
        {
            if (slotDisplayed[obj].item.regenValues[i].regenType == RegenValues.Health)
            {
                player.GetComponent<HP>().AddHP(slotDisplayed[obj].item.regenValues[i].value);
            }
            else {
                player.GetComponent<Energy>().AddEnergy(slotDisplayed[obj].item.regenValues[i].value);
            }
        }

        slotDisplayed[obj].AddAmount(-1);

        if (slotDisplayed[obj].amount <= 0)
        {
            slotDisplayed[obj].RemoveItem();
        }
    }

    public void EquipItem(GameObject obj)
    {
        for (int i = 0; i < equipmentUI.thisInventory.GetSlots.Length; i++)
        {
            InventorySlot tempSlot = AvailableSlotSameType(slotDisplayed[obj].ItemObject);
            if (tempSlot != null)
            {
                thisInventory.SwapItem(slotDisplayed[obj], tempSlot);
                break;
            }
            else if (equipmentUI.thisInventory.GetSlots[i].CanPlaceInSlot(slotDisplayed[obj].ItemObject))
            {
                thisInventory.SwapItem(slotDisplayed[obj], equipmentUI.thisInventory.GetSlots[i]);
                break;
            }
        }
    }

    public InventorySlot AvailableSlotSameType(ItemObject _itemObject)
    {
        for (int i = 0; i < equipmentUI.thisInventory.GetSlots.Length; i++)
        {
            if (equipmentUI.thisInventory.GetSlots[i].CanPlaceInSlot(_itemObject) 
            && equipmentUI.thisInventory.GetSlots[i].ItemObject == null)
            {
                return equipmentUI.thisInventory.GetSlots[i];
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
