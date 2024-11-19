using UnityEngine;

public class ItemFactory: MonoBehaviour
{
    public GameObject[] itemList;

    public void SpawnItem(string name, Vector3 position)
    {
        foreach (GameObject item in itemList)
        {
            if (name == item.name)
            {
                item.transform.position = position;
                item.SetActive(true);

                return;
            }
        }
    }
}
