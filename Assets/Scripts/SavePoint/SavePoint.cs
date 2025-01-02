using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private GameObject player;
    private int clickCount = 0;
    public float delayClickTime;
    private float delayClickTimer;
    public static SavePoint instance {get; private set;}


    private void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        if (delayClickTimer > delayClickTime)
        {
            clickCount = 0;
            delayClickTimer = 0;
        }

        delayClickTimer += Time.deltaTime;
    }

    private void OnMouseDown() {
        clickCount++;

        if (clickCount >= 2 && Vector3.Distance(transform.position, player.transform.position) <= 4)
        {
            player.GetComponent<CharacterMovement>().currentCheckPointPosition = player.transform.position;
            player.gameObject.GetComponentInParent<CharacterData>().SaveData();
            clickCount = 0;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            player.GetComponent<CharacterMovement>().currentCheckPointPosition = player.transform.position;
            other.gameObject.GetComponentInParent<CharacterData>().SaveData();
        }
    }
}
