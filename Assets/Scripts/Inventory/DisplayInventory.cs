using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int x_start;
    public int y_start;

    public int x_space_between_items;
    public int y_space_between_items;
    public int columnNumber;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start() {
        CreateDisplay();
    }

    private void Update() {
        UpdateDisplay();
    }

    public void CreateDisplay() {
        for (int i = 0; i < inventory.container.Count; i++) {
            var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);

            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");

            itemsDisplayed.Add(inventory.container[i], obj);
        }
    }

    public void UpdateDisplay() {
        for (int i=0; i < inventory.container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.container[i]))
            {
                itemsDisplayed[inventory.container[i]].
                    GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);

                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");

                itemsDisplayed.Add(inventory.container[i], obj);
            }
        }
    }

    public Vector3 GetPosition(int index)
    {
        return new Vector3(
            x_start + (x_space_between_items * (index % columnNumber)), 
            y_start + (-y_space_between_items * (index / columnNumber)), 0);
    }
}
