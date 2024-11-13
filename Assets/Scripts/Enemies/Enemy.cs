using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Player Component
    private GameObject player;

    // Enemy Stat
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float speed;
    [SerializeField] private int exp;

    private float stunTime = 0;

    [SerializeField] private float attackCD;
    private float attackTimer = Mathf.Infinity;

    bool isChasing = false;
    bool isDead = false;

    // Component
    private CharacterController controller;
    private Animator animator;

    private MouseUI mouseUI;

    
    void Awake()
    {
        exp = GetComponent<Stat>().lvl * 20;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mouseUI = GameObject.FindGameObjectWithTag("CursorUI").GetComponent<MouseUI>();
    }

    void Update()
    {
        // Hurt and Defeated
        if (animator.GetBool("hurt") || animator.GetBool("meleeAttack"))
            return;

        if (GetComponent<HP>().defeat)
        {
            if (!isDead) 
                Dead();
            return;
        }

        // Stun
        if (stunTime > 0)
        {
            Stun();
            return;
        }

        if (PlayerInRange() && !PlayerInAttackRange())
        {
            isChasing = true;
        }
        else if (!PlayerInRange())
        {
            isChasing = false;
            animator.SetBool("moving", false);
        }
        
        if (PlayerInAttackRange() && attackTimer > attackCD)
        {
            attackTimer = 0;
            isChasing = false;
            animator.SetBool("moving", false);
            MeleeAttack();
        }

        if (isChasing)
        {
            Chase();
        }

        attackTimer += Time.deltaTime;
    }

    // Stun effect
    public void getStun(float time)
    {
        stunTime = time;
        animator.SetBool("stun", true);
    }

    private void Stun()
    {
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            animator.SetBool("stun", false);
        }
    }

    bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= detectRange;
    }

    bool PlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }

    void Chase()
    {
        animator.SetBool("moving", true);
        transform.LookAt(player.transform.position);
        controller.SimpleMove(transform.forward * speed);
    }

    void MeleeAttack()
    {
        animator.SetTrigger("meleeAttack");
    }

    void DamagePlayer()
    {
        if (PlayerInAttackRange() && !Skill.unstoppable)
        {
            player.GetComponent<HP>().TakeDamage(DamageCalculator());
        }
    }

    float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr();
    }

    void Dead()
    {
        isDead = true;

        player.GetComponent<Stat>().AddExp(exp);
        exp = 0;

        player.GetComponent<CharacterCombat>().opponent = null;
        
        animator.SetBool("stun", false);

        CharacterMovement.isAttacking = false;
    }

    public bool getDeadStatus()
    {
        return isDead;
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(mouseUI.mouseOnEnemy, Vector3.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    private void OnMouseDown() {
        if (!isDead)
            player.GetComponent<CharacterCombat>().opponent = gameObject;
    }
}
