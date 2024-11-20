using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    [SerializeField] private float activeRange;
    private GameObject player;
    public bool active = true;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (!active) 
        {
            gameObject.SetActive(false);
        }

        if (active && Vector3.Distance(transform.position, player.transform.position) < activeRange)
        {
            ItemFactory.instance.SpawnItem(itemPrefab.name, transform.position);
            active = false;
            gameObject.SetActive(false);
        }
    }
}
