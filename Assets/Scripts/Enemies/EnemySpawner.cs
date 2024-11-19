using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject player;

    public EnemyFactory factory;
    public float spawnRate;
    private float spawnTimer = Mathf.Infinity;
    public float spawnDistance;

    private bool active = true;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (!active) 
        {
            gameObject.SetActive(false);
            return;
        }

        if (RightCondition())
        {
            spawnTimer = 0;
            factory.SpawnEnemy(monsterPrefab.name, transform.position);
        }

        spawnTimer += Time.deltaTime;
    }

    private bool RightCondition()
    {
        return (spawnTimer >= spawnRate) && (Vector3.Distance(transform.position, player.transform.position) <= spawnDistance);
    }
}
