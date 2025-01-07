using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MerchantInterface : UserInterface
{
    public int x_start;
    public int y_start;

    public int x_space_between_items;
    public int y_space_between_items;
    public int columnNumber;

    // Audio
    [SerializeField] private AudioClip purchaseSound;

    public static bool isShopping;

    public void OnDisable() {
        isShopping = false;
        PauseMenuUI.isPausing = false;
    }

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

        // Buy Item
        BuyItem(obj);
    }

    public void BuyItem(GameObject obj)
    {
        // Buy the item
        if (player.GetComponent<CharacterInventory>().GetCash() >= slotDisplayed[obj].item.buyCost)
        {
            AudioManager.instance.PlaySound(purchaseSound);
            player.GetComponent<CharacterInventory>().AddCash(slotDisplayed[obj].item.buyCost*-1);
            player.GetComponent<CharacterInventory>().inventory.AddItem(new Item(slotDisplayed[obj].ItemObject), 1);
        }
    }

    public new void GetItemInfo(GameObject obj)
    {
        int slotIndex = slotDisplayed.Values.ToList().IndexOf(slotDisplayed[obj]);
        ItemInfoUI.instance.SetInfoShop(
            thisInventory.GetSlots[slotIndex].ItemObject, 
            thisInventory.GetSlots[slotIndex].item);
    }

    private Vector3 GetPosition(int index)
    {
        return new Vector3(
            x_start + (x_space_between_items * (index % columnNumber)), 
            y_start + (-y_space_between_items * (index / columnNumber)), 0);
    }
}
