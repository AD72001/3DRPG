using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;

public class HP : MonoBehaviour
{
    // HP
    [SerializeField] public float startingHP;
    public float currentHP;

    // IFrames
    [SerializeField] private float iFramesDuration;

    // Audio
    // [SerializeField] private AudioClip hurtSound;
    // [SerializeField] private AudioClip defeatSound;

    private Animator animator;
    public string saveLocation = "/HP.sav";
    
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
            _dmg = _dmg * 70 / (70 + GetComponent<Stat>().GetDef()*1.0f);

            currentHP = Mathf.Clamp(currentHP - _dmg, 0, startingHP);

            if (currentHP > 0)
            {
                StartCoroutine(Invulnerable());
            }
            else
            {
                if (gameObject.CompareTag("Player"))
                {
                    gameObject.GetComponent<NavMeshAgent>().isStopped=true;
                    CharacterMovement.isAttacking=false;
                    gameObject.GetComponent<CharacterCombat>().opponent = null;
                }
                Dead();
            }
        }
    }

    public void Dead()
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
        gameObject.SetActive(false);

        Debug.Log("GameObject: " + gameObject.transform.position);
        AddHP(startingHP);

        animator.ResetTrigger("dead");
        animator.Play("Idle");

        defeat = false;
        Skill.isUsingSkill = false;

        Vector3 spawnLocation = gameObject.GetComponent<CharacterMovement>().currentCheckPointPosition;

        gameObject.transform.position = spawnLocation;

        gameObject.GetComponent<CharacterMovement>().SetPosition(spawnLocation);
        Physics.SyncTransforms();
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
        EnemyFactory.instance.DeactiveAll();

        gameObject.SetActive(true);

        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        gameObject.GetComponent<NavMeshAgent>().nextPosition = spawnLocation;
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

    [ContextMenu("Save")]
    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, saveLocation));
        bf.Serialize(file, saveData);
        file.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, saveLocation)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, saveLocation), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
}
