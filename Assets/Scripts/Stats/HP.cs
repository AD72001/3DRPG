using System;
using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    // HP
    [SerializeField] public float startingHP;
    [NonSerialized] public float currentHP;

    // IFrames
    [SerializeField] private float iFramesDuration;

    // Audio
    // [SerializeField] private AudioClip hurtSound;
    // [SerializeField] private AudioClip defeatSound;

    private Animator animator;
    
    public bool defeat {get; private set;}
    private bool isInvul = false;

    [SerializeField] private Behaviour[] components;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable() {
        if (GetComponent<Stat>())
            startingHP = GetComponent<Stat>().GetVit() * 2;

        currentHP = startingHP;

        defeat = false;
    }
    
    public void TakeDamage(float _dmg)
    {
        if (!isInvul)
        {
            // Limit current HP to 0 -> maximum
            _dmg = _dmg * 100 / (100 + GetComponent<Stat>().GetDef()*1.0f);

            currentHP = Mathf.Clamp(currentHP - _dmg, 0, startingHP);

            if (gameObject.CompareTag("Player"))
            {
                Skill.isUsingSkill = false;
                CharacterMovement.isAttacking = false;
            }

            if (currentHP > 0)
            {
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

    public float GetHP()
    {
        return currentHP;
    }

    public void AddHPMax(float _addHPMax)
    {
        startingHP += _addHPMax;
        currentHP = Mathf.Clamp(currentHP + _addHPMax, 0, startingHP);
    }

    public float GetHPMax()
    {
        return startingHP;
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
