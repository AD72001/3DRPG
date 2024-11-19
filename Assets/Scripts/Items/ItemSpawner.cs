using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public ItemFactory factory;
    [SerializeField] private float activeRange;
    private GameObject player;
    public bool active = true;

    private void Start() {
        if (!active || Vector3.Distance(transform.position, player.transform.position) >= activeRange) 
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);

    }

    private void OnEnable() {
        factory.SpawnItem(itemPrefab.name, transform.position);
        active = false;
    }
}
