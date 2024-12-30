using UnityEngine;

public class RangeSkill_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skill_1>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skill_1>().enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<Skill_1>().enemies.Contains(other.gameObject) && 
            other.gameObject.GetComponent<HP>().defeat)
        {
            GetComponentInParent<Skill_1>().enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Skill_1>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skill_1>().enemies.Remove(other.gameObject);
        }
    }
}
