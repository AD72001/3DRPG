using UnityEngine;

public class RangeSkill_3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GetComponentInParent<Third_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Third_Skill>().enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<Third_Skill>().enemies.Contains(other.gameObject) && 
            other.gameObject.GetComponent<Enemy>().getDeadStatus())
        {
            GetComponentInParent<Third_Skill>().enemies.Remove(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy") && GetComponentInParent<Third_Skill>().enemies.Contains(other.gameObject))
        {
            GetComponentInParent<Third_Skill>().enemies.Remove(other.gameObject);
        }
    }
}
