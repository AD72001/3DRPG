using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    [SerializeField] private GameObject item;
    [SerializeField] private float activeRange;
    private GameObject player;
    public bool pickedUp = false;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (pickedUp)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) < activeRange)
            {
                item = ItemFactory.instance.SpawnItem(itemPrefab.name, transform.position);        
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            pickedUp = true;
        }
    }
}
