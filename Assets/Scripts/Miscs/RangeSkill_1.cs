using UnityEngine;

public class RangeSkill_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<First_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<First_Skill>().enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<First_Skill>().enemies.Contains(other.gameObject) && 
            other.gameObject.GetComponent<Enemy>().getDeadStatus())
        {
            GetComponentInParent<First_Skill>().enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<First_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<First_Skill>().enemies.Remove(other.gameObject);
        }
    }
}
