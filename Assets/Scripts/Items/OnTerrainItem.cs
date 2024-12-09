using UnityEditor;
using UnityEngine;

public class OnTerrainItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject itemObject;
    [SerializeField] private int amount = 1;

    public void OnAfterDeserialize() {}

    public void OnBeforeSerialize()
    {
        # if UNITY_EDITOR
        if (itemObject == null)
            return;
        GetComponentInChildren<SpriteRenderer>().sprite = itemObject.iconDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        # endif
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<CharacterInventory>().inventory.AddItem(new Item(itemObject), amount))
                gameObject.SetActive(false);
        }
    }

}
