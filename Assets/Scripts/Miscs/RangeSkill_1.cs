using UnityEngine;

public class RangeSkill_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().first_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().first_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().first_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().first_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Skills>().first_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().first_enemies.Remove(other.gameObject);
        }
    }
}
