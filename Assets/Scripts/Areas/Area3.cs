using UnityEngine;

public class Area3 : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private GameObject player;
    
    // Gate
    [SerializeField] private GameObject gate;
    public GameObject closePoint;
    public GameObject openPoint;

    // Goal point
    [SerializeField] private GameObject goal;

    // Status
    private bool bossAlive = true;
    private bool isClosing = true;
    private bool isLocked = false;

    // Locked Conditions
    public StaticEnemy magicCircle1;
    public StaticEnemy magicCircle2;

    // Victory
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private AudioClip victorySound;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            //Player enter -> activate boss
            boss.SetActive(true);
            boss.GetComponent<WizardEnemy>().active = true;

            //Lock player up
            isClosing = true;
            isLocked = true;
        }
    }

    private void Update() {

        if (boss.GetComponent<HP>().defeat && bossAlive)
        {
            GoalActive();
            return;
        }

        if (player.GetComponent<HP>().defeat)
        {
            boss.GetComponent<WizardEnemy>().active = false;
        }

        if (isLocked)
        {
            if (isClosing && Vector3.Distance(transform.position, closePoint.transform.position) < 0.2f)
            {
                return;
            }
            else
            {
                CloseGate();
                return;
            }
        }

        if (magicCircle1.getDeadStatus() && magicCircle2.getDeadStatus())
            isClosing = false;
        
        if (isClosing)
            CloseGate();
        else
            OpenGate();

    }

    public void GoalActive()
    {
        bossAlive = false;

        goal.SetActive(true);
    }

    private void CloseGate()
    {
        gate.transform.position = Vector3.MoveTowards(gate.transform.position, closePoint.transform.position, Time.deltaTime*10.0f);
    }

    private void OpenGate()
    {
        gate.transform.position = Vector3.MoveTowards(gate.transform.position, openPoint.transform.position, Time.deltaTime*1.0f);
    }
}
