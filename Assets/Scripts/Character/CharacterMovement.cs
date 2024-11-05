using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Player Movement
    public float moveSpeed;
    public float turnSpeed;
    Vector3 position;

    public static bool isAttacking = false;

    //Component
    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController controller;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((Skill.isUsingSkill && !Skill.unstoppable)
            || animator.GetBool("hurt")
            || animator.GetBool("dead"))
        {
            position = transform.position;
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
            
            controller.SimpleMove(transform.forward * moveSpeed);
        }
        else {
            animator.SetBool("moving", false);
        }
    }

    public void SetPosition(Vector3 target)
    {
        position = target;
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

    private void OnApplicationQuit() {
        GetComponent<Inventory>().inventory.container.Clear();
    }
}
