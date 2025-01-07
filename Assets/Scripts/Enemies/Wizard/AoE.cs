
using UnityEngine;

public class AoE : MonoBehaviour
{
    [SerializeField] private float dmgRate;
    public float dmgTimer;
    
    [SerializeField] private GameObject user;
    [SerializeField] private float mod_int;

    private void OnEnable() {
        dmgTimer = dmgRate;
    }

    private void OnDisable() 
    {
        if (user != null)
        {
            user.GetComponent<WizardEnemy>().FinishAttack();
        }
    }

    private void Update() {
        dmgTimer += Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (dmgTimer >= dmgRate)
            {
                other.gameObject.GetComponent<HP>().TakeDamage(user.GetComponent<Stat>().GetInt() * mod_int);
                dmgTimer = 0;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (dmgTimer >= dmgRate)
            {
                other.gameObject.GetComponent<HP>().TakeDamage(user.GetComponent<Stat>().GetInt() * mod_int);
                dmgTimer = 0;
            }
        }
    }
}
