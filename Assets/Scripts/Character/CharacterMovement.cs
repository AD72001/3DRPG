using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    // Player Movement
    private MouseUI mouseUI;
    public float moveSpeed;
    public float turnSpeed;
    Vector3 position;
    public string[] savePosition;
    private NavMeshAgent agent;

    // Player Status
    private float stunTime;

    public static bool isAttacking = false;

    //Component
    [SerializeField] private Camera cam;
    private CharacterController controller;
    private Animator animator;

    // Effect on click
    [SerializeField] private GameObject effectOnClick;

    public string saveLocation = "/position.sav";

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        agent = GetComponent<NavMeshAgent>();

        savePosition = new string[3] {"0", "0", "0"};

        if (position == Vector3.zero) 
            position = transform.position;
        
        animator = GetComponent<Animator>();
        mouseUI = GameObject.FindGameObjectWithTag("CursorUI").GetComponent<MouseUI>();
    }

    void Update()
    {
        if ((Skill.isUsingSkill && !Skill.unstoppable)
            || animator.GetBool("stun")
            || animator.GetBool("dead"))
        {
            if (GetComponent<HP>().defeat)
            {
                return;
            }

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

        if (Physics.Raycast(ray, out hit, 2000))
        {
            // Instantiate(effectOnClick, 
            //     new Vector3(hit.point.x, hit.point.y, hit.point.z), 
            //     Quaternion.identity);

            if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Enemy"))
            {
                animator.SetBool("moving", true);
                isAttacking = false;
                CharacterCombat.normalAtk = false;
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.gameObject.GetComponent<Enemy>().getDeadStatus())
                    return;

                isAttacking = true;
                CharacterCombat.normalAtk = true;
                GetComponent<CharacterCombat>().opponent = hit.collider.gameObject;
            }
        }
    }

    public void MoveToPosition()
    {
        if (Vector3.Distance(new Vector3(position.x, 0, position.z), 
            new Vector3(transform.position.x, 0, transform.position.z)) > 1f)
        {
            Vector3 lookPosition = new Vector3(position.x, transform.position.y, position.z);

            transform.LookAt(lookPosition);
            
            //controller.SimpleMove(transform.forward * moveSpeed);
            agent.destination = position;
        }
        else {
            animator.SetBool("moving", false);
        }
    }

    public void SetPosition(Vector3 target)
    {
        position = target;
        agent.destination = position;
    }

    // Stun effect
    public void getStun(float time)
    {
        if (GetComponent<HP>().defeat) return;
        
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

        SetPosition(transform.position);
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
