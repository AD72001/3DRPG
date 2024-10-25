using UnityEngine;

public class InBoxRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log(GetComponentInParent<Skills>().enemies);
        if (other.CompareTag("Enemy") 
            && !GetComponentInParent<Skills>().enemies.Contains(other.gameObject))
            GetComponentInParent<Skills>().enemies.Add(other.gameObject);       
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy"))
            GetComponentInParent<Skills>().enemies.Remove(other.gameObject);
    }
}
