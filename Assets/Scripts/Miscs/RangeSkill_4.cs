using UnityEngine;

public class RangeSkill_4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Fourth_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Fourth_Skill>().enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<Fourth_Skill>().enemies.Contains(other.gameObject) && 
            other.gameObject.GetComponent<Enemy>().getDeadStatus())
        {
            GetComponentInParent<Fourth_Skill>().enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Fourth_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Fourth_Skill>().enemies.Remove(other.gameObject);
        }
    }
}
