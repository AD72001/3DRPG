using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    // Player Movement
    private MouseUI mouseUI;
    public float turnSpeed;
    Vector3 position;
    [SerializeField] public Vector3 currentCheckPointPosition;
    public string[] savePosition;
    private NavMeshAgent agent;

    // Player Status
    private float stunTime;

    public static bool isAttacking = false;

    //Component
    [SerializeField] private Camera cam;
    private CharacterController controller;
    private Animator animator;

    // Audio
    [SerializeField] private AudioClip stunSound;

    public string saveLocation = "/position.sav";

    public static CharacterMovement instance {get; private set; }

    private void Awake() {
        instance = this;

        controller = GetComponent<CharacterController>();

        agent = GetComponent<NavMeshAgent>();

        savePosition = new string[3] {"0", "0", "0"};

        if (position == Vector3.zero) 
            position = transform.position;

        if (currentCheckPointPosition == Vector3.zero)
            currentCheckPointPosition = transform.position;
        
        animator = GetComponent<Animator>();
        mouseUI = GameObject.FindGameObjectWithTag("CursorUI").GetComponent<MouseUI>();
    }

    void Update()
    {
        if (Time.timeScale < 1)
            return;

        if (GetComponent<HP>().defeat)
        {
            return;
        }

        if ((Skill.isUsingSkill && !Skill.unstoppable)
            || animator.GetBool("stun"))
        {
            if (stunTime > 0)
            {
                Stun();
            }

            SetPosition(transform.position);
            animator.SetBool("moving", false);
            return;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            // Locate the location of the click -> move or attack
            LocatePosition();
        }      

        if (Input.GetMouseButtonDown(0))
        {
            CheckIfExistObject();
        }  

        // Move Characters to the location
        MoveToPosition();
    }

    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(ray, out hit, 2000))
        {
            GameUI.instance.ActivateClickEffect();
            GameUI.instance.SetClickEffectPosition(new Vector3(hit.point.x, hit.point.y, hit.point.z));

            if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy"))
            {
                NavMeshPath navMeshPath = new NavMeshPath();
                isAttacking = false;
                CharacterCombat.normalAtk = false;

                if (agent.CalculatePath(new Vector3(hit.point.x, hit.point.y, hit.point.z), navMeshPath) 
                    && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    if (PathLength(navMeshPath) > 20)
                        return;
                        
                    position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    SetPosition(position);
                }

                return;
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.gameObject.GetComponent<HP>().defeat)
                    return;

                isAttacking = true;
                CharacterCombat.normalAtk = true;
                GetComponent<CharacterCombat>().opponent = hit.collider.gameObject;
            }
        }
    }

    public void MoveToPosition()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("moving", false);
                    return;
                }
            }
        }

        if (Vector3.Distance(new Vector3(position.x, 0, position.z), 
            new Vector3(transform.position.x, 0, transform.position.z)) > 0.1f)
        {
            agent.destination = position;
            animator.SetBool("moving", true);
        }
        else {
            animator.SetBool("moving", false);
        }
    }

    private float PathLength(NavMeshPath path) {
        if (path.corners.Length < 2)
            return 0;
        
        float length = 0.0F;
        for (int i = 1; i < path.corners.Length; i++) {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }

    public void SetPosition(Vector3 target)
    {
        position = target;
        if (agent) agent.destination = position;
    }

    // Stun effect
    public void getStun(float time)
    {
        if (GetComponent<HP>().defeat) return;
        
        stunTime = time;
        AudioManager.instance.PlaySound(stunSound);
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

    void CheckIfExistObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (!hit.collider.CompareTag("Enemy"))
            {
                GetComponent<CharacterCombat>().opponent = null;
            }
        }        
    }
    public void SaveData()
    {
        savePosition = new string[] {transform.position.x.ToString(), transform.position.y.ToString(), transform.position.z.ToString()};

        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, saveLocation));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, saveLocation)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, saveLocation), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }

        transform.position = new Vector3(float.Parse(savePosition[0]),
            float.Parse(savePosition[1]),
            float.Parse(savePosition[2]));


        GameUI.instance.SetClickEffectPosition(transform.position);
        GameUI.instance.DeactivateClickEffect();

        Skill.isUsingSkill = false;

        Physics.SyncTransforms();
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);

        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        gameObject.GetComponent<NavMeshAgent>().nextPosition = transform.position;
        gameObject.GetComponent<NavMeshAgent>().destination = transform.position;

        position = transform.position;
    }

    // Change Later
    private void OnMouseOver() {
        Cursor.SetCursor(mouseUI.mouseOnCharacter, Vector3.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    private void OnApplicationQuit() {
        GetComponent<CharacterInventory>().inventory.Clear();
        GetComponent<CharacterInventory>().equipment.Clear();
    }
}
