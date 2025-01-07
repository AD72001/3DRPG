using UnityEngine;

public class Merchant : MonoBehaviour
{
    public InventoryObject merchantInventory;
    public GameObject merchantInventoryUI;
    public ItemObject[] itemObjectList;

    private int clickCount = 0;
    public float delayClickTime;
    private float delayClickTimer;

    private void Awake() {
        foreach (ItemObject itemObject in itemObjectList)
        {
            if (merchantInventory.ItemInInventory(itemObject.data) != null)
                continue;

            merchantInventory.AddItem(new Item(itemObject), 1);
        }
        
        // merchantInventoryUI.GetComponent<MerchantInterface>().slotDisplayed.UpdateSlotOnDisplay();
    }

    private void Update() {
        if (delayClickTimer > delayClickTime)
        {
            clickCount = 0;
            delayClickTimer = 0;
        }

        if (!IsShopping() && !PauseMenuUI.isPausing)
        {
            Time.timeScale = 1;
        }

        if (IsShopping())
        {
            Time.timeScale = 0;
            PauseMenuUI.isPausing = true;
        }

        delayClickTimer += Time.deltaTime;
    }

    private void OnMouseDown() {
        clickCount++;

        if (clickCount >= 2)
        {
            MerchantInterface.isShopping = true;
            clickCount = 0;
            // Pause game
            Time.timeScale = 0;
            PauseMenuUI.isPausing = true;
            // Open Inventory UI of merchant and player
            merchantInventoryUI.SetActive(true);
        }
    }

    private bool IsShopping()
    {
        foreach (Transform child in merchantInventoryUI.transform.parent)
        {
            if (child.gameObject.activeSelf) return true;
        }

        return false;
    }
}
