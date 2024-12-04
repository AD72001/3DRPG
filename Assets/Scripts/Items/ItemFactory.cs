using UnityEngine;

public class ItemFactory: MonoBehaviour
{
    public GameObject[] itemList;

    public static ItemFactory instance {get; private set;}

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject SpawnItem(string name, Vector3 position)
    {
        foreach (GameObject item in itemList)
        {
            if (name == item.name)
            {
                item.transform.position = position;
                item.SetActive(true);

                return item;
            }
        }

        return null;
    }
}
