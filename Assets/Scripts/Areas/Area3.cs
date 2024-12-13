using UnityEngine;

public class Area3 : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject gate;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            //Player enter -> activate boss
            boss.GetComponent<WizardEnemy>().active = true;

            //Lock player up
        }
    }
}
