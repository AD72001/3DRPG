using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotInterface : MonoBehaviour, IPointerClickHandler
{
    private InventoryType _inventoryType;

    private void Awake() {
        _inventoryType = transform.parent.GetComponent<DynamicInterface>() == null ? InventoryType.Equipment : InventoryType.Inventory;
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
                default:
                    break;
            }
        }
    }
}
