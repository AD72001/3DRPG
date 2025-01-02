using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public static bool isUsingSkill;
    public static bool unstoppable;

    // Stats related to the skill
    public int unlockLevel;
    [SerializeField] protected GameObject[] effects;
    [SerializeField] protected Collider range;
    [SerializeField] protected float mod_str;
    [SerializeField] protected float mod_int;
    [SerializeField] protected float mod_def;
    [SerializeField] protected float mod_vit;
    [SerializeField] protected float cost; // energy cost
    public float CD;
    public float timer;

    // Skill information
    [SerializeField] protected string skillName;
    [SerializeField] protected string desc;

    // Keycode
    [SerializeField] protected KeyCode keyCode;

    // Enemies list
    [SerializeField] public List<GameObject> enemies;

    // Components
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject skillUI;

    private void OnEnable() {
        animator = GetComponent<Animator>();
        
        if (GetComponent<Stat>().level < unlockLevel)
        {
            enabled = false;
            skillUI.SetActive(false);
        }
        else {
            enabled = true;
            skillUI.SetActive(true);
        }
    }

    private void Update() {
        if (isUsingSkill || GetComponent<HP>().defeat)
        {
            return;
        }
        
        if (Input.GetKeyDown(keyCode)) {
            KeyPressed();
        }

        timer = Mathf.Clamp(timer - Time.deltaTime, 0, CD);
    }

    public void KeyPressed()
    {
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

    protected virtual void UseSkill() {}
    protected virtual void SkillEffect() {}

    public string GetDescription()
    {
        return desc;
    }

    public string GetName()
    {
        return skillName;
    }

    protected void FinishSkill()
    {
        CharacterMovement.isAttacking = false;
        isUsingSkill = false;

        unstoppable = false;
    }

    public void UnlockSkill()
    {
        enabled = true;
        skillUI.SetActive(true);
    }
}
