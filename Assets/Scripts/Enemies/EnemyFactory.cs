using UnityEngine;

public class EnemyFactory: MonoBehaviour
{
    public GameObject[] enemyList;

    public void SpawnEnemy(string name, Vector3 position)
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy.name.Split("_")[0] == name && !enemy.activeSelf)
            {
                enemy.transform.position = position;
                enemy.SetActive(true);

                return;
            }
        }
    }
}
