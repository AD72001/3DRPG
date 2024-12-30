using UnityEngine;

public class RangeSkill_4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Skill_4>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skill_4>().enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<Skill_4>().enemies.Contains(other.gameObject) && 
            other.gameObject.GetComponent<HP>().defeat)
        {
            GetComponentInParent<Skill_4>().enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Skill_4>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Skill_4>().enemies.Remove(other.gameObject);
        }
    }
}
