using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().playerInAttackRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().playerInAttackRange = null;
        }
    }
}
