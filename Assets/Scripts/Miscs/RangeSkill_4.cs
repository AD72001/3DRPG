using UnityEngine;

public class RangeSkill_4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().fourth_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().fourth_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().fourth_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().fourth_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Skills>().fourth_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().fourth_enemies.Remove(other.gameObject);
        }
    }
}
