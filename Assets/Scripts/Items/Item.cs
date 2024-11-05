using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemObject item;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Inventory>().inventory.AddItem(item, amount);
            gameObject.SetActive(false);
        }
    }

}
