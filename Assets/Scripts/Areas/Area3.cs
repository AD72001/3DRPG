using UnityEngine;

public class Area3 : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject gate;
    public GameObject closePoint;
    public GameObject openPoint;
    private bool isClosing = true;
    private bool isLocked = false;

    public StaticEnemy magicCircle1;
    public StaticEnemy magicCircle2;

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
        // Debug.Log(magicCircle1.getDeadStatus() +  " " + magicCircle2.getDeadStatus() + " " + isClosing + " " + isLocked);
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

    private void CloseGate()
    {
        gate.transform.position = Vector3.MoveTowards(gate.transform.position, closePoint.transform.position, Time.deltaTime*10.0f);
    }

    private void OpenGate()
    {
        gate.transform.position = Vector3.MoveTowards(gate.transform.position, openPoint.transform.position, Time.deltaTime*1.0f);
    }
}
