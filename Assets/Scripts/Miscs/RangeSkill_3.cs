using UnityEngine;

public class RangeSkill_3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().third_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().third_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skills>().third_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().third_enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Skills>().third_enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skills>().third_enemies.Remove(other.gameObject);
        }
    }
}
