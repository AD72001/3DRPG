using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int str;
    [SerializeField] private int def;
    [SerializeField] private int vit;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Stat>().AddStr(str);
            other.GetComponent<Stat>().AddDef(def);
            other.GetComponent<Stat>().AddVit(vit);

            gameObject.SetActive(false);
        }
    }
}
