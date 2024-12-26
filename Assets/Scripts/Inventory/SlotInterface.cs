using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotInterface : MonoBehaviour, IPointerClickHandler
{
    private InventoryType _inventoryType;

    private void Awake() {
        _inventoryType = transform.parent.GetComponent<DynamicInterface>() == null ? InventoryType.Equipment : InventoryType.Inventory;

        if (transform.parent.GetComponent<DynamicInterface>())
            _inventoryType = InventoryType.Inventory;
        else if (transform.parent.GetComponent<StaticInterface>())
            _inventoryType = InventoryType.Equipment;
        else if (transform.parent.GetComponent<MerchantInterface>())
            _inventoryType = InventoryType.Merchant;
        else
            _inventoryType = InventoryType.Storage;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right || pointerEventData.clickCount >= 2)
        {
            switch (_inventoryType)
            {
                case InventoryType.Inventory:
                    transform.GetComponentInParent<DynamicInterface>().OnClick(gameObject);
                    break;
                case InventoryType.Equipment:
                    transform.GetComponentInParent<StaticInterface>().OnClick(gameObject);
                    break;
                case InventoryType.Merchant:
                    transform.GetComponentInParent<MerchantInterface>().OnClick(gameObject);
                    break;
                default:
                    break;
            }

            pointerEventData.clickCount = 0;
        }

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            switch (_inventoryType)
            {
                case InventoryType.Inventory:
                    transform.GetComponentInParent<DynamicInterface>().GetItemInfo(gameObject);
                    break;
                case InventoryType.Equipment:
                    transform.GetComponentInParent<StaticInterface>().GetItemInfo(gameObject);
                    break;
                case InventoryType.Merchant:
                    transform.GetComponentInParent<MerchantInterface>().GetItemInfo(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
