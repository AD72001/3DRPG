using UnityEngine;

public class EnemyFactory: MonoBehaviour
{
    public GameObject[] enemyList;

    public static EnemyFactory instance {get; private set;}

    private void Awake() {
        if (instance == null) instance = this;
    }

    public GameObject SpawnEnemy(string name, Vector3 position)
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy.name.Split("_")[0] == name && !enemy.activeSelf)
            {
                enemy.transform.position = position;
                enemy.SetActive(true);

                return enemy;
            }
        }

        return null;
    }

    public void DeactiveAll()
    {
        foreach (GameObject enemy in enemyList)
        {
            enemy.SetActive(false);
        }
    }
}
