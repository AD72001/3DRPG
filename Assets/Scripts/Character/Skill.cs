using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public static bool isUsingSkill;
    public static bool unstoppable;

    // Stats related to the skill
    [SerializeField] protected GameObject[] effects;
    [SerializeField] protected Collider range;
    [SerializeField] protected float mod;
    [SerializeField] protected float cost; // energy cost
    public float CD;
    public float timer;

    // Keycode
    [SerializeField] protected KeyCode keyCode;

    // Enemies list
    [SerializeField] public List<GameObject> enemies;

    // Components
    [SerializeField] protected Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (isUsingSkill)
        {
            return;
        }
        
        if (Input.GetKeyDown(keyCode)) {

            if (timer <= 0 && GetComponent<Energy>().GetEnergy() >= cost)
            {
                CharacterCombat.normalAtk = false;
                UseSkill();
                timer = CD;
                GetComponent<Energy>().AddEnergy(-cost);
            }
            else
            {
                Debug.Log("Skill not available");
            }

        }

        timer = Mathf.Clamp(timer - Time.deltaTime, 0, CD);
    }

    protected virtual void UseSkill() {}
    protected virtual void SkillEffect() {}

    protected void FinishSkill()
    {
        CharacterMovement.isAttacking = false;
        isUsingSkill = false;

        unstoppable = false;
    }
}
