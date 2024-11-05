using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    // HP
    [SerializeField] public float startingHP;
    public float currentHP { get; private set; }

    // IFrames
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    // Audio
    // [SerializeField] private AudioClip hurtSound;
    // [SerializeField] private AudioClip defeatSound;

    private Animator animator;
    
    public bool defeat {get; private set;}
    private bool isInvul = false;

    [SerializeField] private Behaviour[] components;

    private void Awake()
    {
        if (GetComponent<Stat>())
            startingHP = GetComponent<Stat>().vit * 2;

        currentHP = startingHP;

        defeat = false;

        animator = gameObject.GetComponent<Animator>();
    }
    
    public void TakeDamage(float _dmg)
    {
        if (!isInvul)
        {
            // Limit current HP to 0 -> maximum
            _dmg = _dmg * 100 / (100 + GetComponent<Stat>().def*1.0f);

            currentHP = Mathf.Clamp(currentHP - _dmg, 0, startingHP);

            if (gameObject.CompareTag("Player"))
            {
                Skill.isUsingSkill = false;
                CharacterMovement.isAttacking = false;
            }

            if (currentHP > 0)
            {
                animator.SetTrigger("hurt");
                StartCoroutine(Invulnerable());
            }
            else
            {
                PlayerDead();
            }
        }
    }

    public void PlayerDead()
    {
        if (!defeat)
        {
            defeat = true;

            foreach (Behaviour comp in components)
            {
                if (comp != null)
                    comp.enabled = false;
            }

            animator.SetTrigger("dead");

            // Deactivate();
        }
    }

    public void AddHP(float _addHP)
    {
        currentHP = Mathf.Clamp(currentHP + _addHP, 0, startingHP);
    }

    public void AddHPMax(float _addHPMax)
    {
        startingHP += _addHPMax;
        currentHP = Mathf.Clamp(currentHP + _addHPMax, 0, startingHP);
    }

    public void Respawn()
    {
        foreach (Behaviour comp in components)
        {
            if (comp != null)
                comp.enabled = true;
        }

        AddHP(startingHP);

        animator.ResetTrigger("dead");
        animator.Play("Idle");

        StartCoroutine(Invulnerable());

        defeat = false;
    }

    // IFrames function
    private IEnumerator Invulnerable()
    {
        isInvul = true;

        Physics2D.IgnoreLayerCollision(8, 9, true);

        yield return new WaitForSeconds(iFramesDuration);

        Physics2D.IgnoreLayerCollision(8, 9, false);

        isInvul = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
