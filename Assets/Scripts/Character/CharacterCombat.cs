
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    // Components
    private Animator animator;
    private CharacterMovement movement;
    public GameObject opponent;

    // Stats
    [SerializeField] private float range;
    public static bool normalAtk;


    private void Awake() {
        movement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        if (CharacterMovement.isAttacking && normalAtk)
        {
            if (opponent) 
            {
                transform.LookAt(opponent.transform.position);

                if (!InRange())
                {
                    movement.SetPosition(opponent.transform.position);
                    animator.SetBool("moving", true);
                    movement.MoveToPosition();
                }
                else
                {
                    movement.SetPosition(transform.position);
                    animator.SetBool("moving", false);
                    animator.SetTrigger("attack_01");
                }
            }
        }

        animator.SetBool("attacking", CharacterMovement.isAttacking);
    }

    private bool InRange()
    {
        if (opponent.GetComponent<CapsuleCollider>())
            return Vector3.Distance(transform.position, opponent.transform.position) 
            <= (range + opponent.GetComponent<CapsuleCollider>().radius);
        return Vector3.Distance(transform.position, opponent.transform.position) <= range;
    }

    void DamageOpponent()
    {
        if (!opponent) return;

        if (InRange())
        {
            opponent.GetComponent<HP>().TakeDamage(DamageCalculator());
        }
    }

    float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr()*1.0f;
    }
}
