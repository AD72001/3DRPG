using UnityEditor;
using UnityEngine;

public class OnTerrainItem : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private ItemObject item;
    [SerializeField] private int amount = 1;

    public void OnAfterDeserialize() {}

    public void OnBeforeSerialize()
    {
        # if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.iconDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        # endif
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<CharacterInventory>().inventory.AddItem(new Item(item), amount))
                gameObject.SetActive(false);
        }
    }

}
