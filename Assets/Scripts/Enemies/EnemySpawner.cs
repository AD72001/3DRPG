using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject player;
    private GameObject enemy;

    public float spawnRate;
    private float spawnTimer = Mathf.Infinity;
    public float spawnDistance;

    [SerializeField] private bool active = true;
    [SerializeField] private bool unique = false;
    [SerializeField] private bool spawned = false;
    private bool defeat = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (!active) 
        {
            gameObject.SetActive(false);
        }

        if (enemy != null) defeat = enemy.GetComponent<HP>().defeat;

        if (unique && defeat)
        {
            active = false;
            return;
        }

        if (RightCondition())
        {
            spawnTimer = 0;
            spawned = true;
            enemy = EnemyFactory.instance.SpawnEnemy(monsterPrefab.name, transform.position);
        }

        if (enemy != null && !enemy.activeSelf && !enemy.GetComponent<HP>().defeat)
        {
            spawned = false;
        }

        spawnTimer += Time.deltaTime;
    }

    private bool RightCondition()
    {
        if (unique && spawned)
            return false;

        return (spawnTimer >= spawnRate) && (Vector3.Distance(transform.position, player.transform.position) <= spawnDistance);
    }

    public bool ReActiveCondition()
    {
        return unique && defeat;
    }
}
