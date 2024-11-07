using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    protected Dictionary<GameObject, InventorySlot> slotDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start() {

        for (int i=0; i < inventory.container.items.Length; i++) 
        {
            inventory.container.items[i].parent = this;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate{ OnEnterInventory(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate{ OnExitInventory(gameObject); });

        CreateSlots();
    }

    private void Update() {
        slotDisplayed.UpdateSlotOnDisplay();
    }

    public abstract void CreateSlots();

    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredObj = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredObj = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempObject(obj);
    }

    public GameObject CreateTempObject(GameObject obj)
    {
        GameObject tempObject = null;

        if (slotDisplayed[obj].item.Id >= 0)
        {
            tempObject = new GameObject();
            var rt = tempObject.AddComponent<RectTransform>();

            tempObject.transform.SetParent(transform.parent);

            var img = tempObject.AddComponent<Image>();
            img.sprite = slotDisplayed[obj].ItemObject.iconDisplay; // icon

            rt.sizeDelta = img.GetComponent<RectTransform>().sizeDelta; // size
            rt.transform.localScale = new Vector3(1, 1, 1);

            img.raycastTarget = false;
        }

        return tempObject;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        // Check if item currently dragged by the mouse is inside an inventory or not

        // Remove item
        if (MouseData.interfaceMouseIsAt == null)
        {
            slotDisplayed[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredObj)
        {
            InventorySlot slotHoveredData = MouseData.interfaceMouseIsAt.slotDisplayed[MouseData.slotHoveredObj];

            inventory.SwapItem(slotHoveredData, slotDisplayed[obj]);
        }
    }

    public void OnDragging(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnEnterInventory(GameObject obj)
    {
        MouseData.interfaceMouseIsAt = obj.GetComponent<UserInterface>();
    }

    public void OnExitInventory(GameObject obj)
    {
        MouseData.interfaceMouseIsAt = null;
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsAt;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredObj;
}

public static class ExtensionMethods 
{
    public static void UpdateSlotOnDisplay(this Dictionary<GameObject, InventorySlot> _slotsDisplayed)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsDisplayed)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite 
                    = _slot.Value.ItemObject.iconDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);

                if (_slot.Key.GetComponentInChildren<TextMeshProUGUI>())
                    _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);

                if (_slot.Key.GetComponentInChildren<TextMeshProUGUI>())
                    _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
