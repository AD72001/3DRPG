using UnityEngine;

public class CharacterInventory: MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    [SerializeField] private int cash;

    public int GetCash()
    {
        return cash;
    }

    public void AddCash(int value)
    {
        cash += value;
    }
}
