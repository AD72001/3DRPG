using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private GameObject player;
    private int clickCount = 0;
    public float delayClickTime;
    private float delayClickTimer;
    public static SavePoint instance {get; private set;}
    private Vector3 currentCheckPointPosition;

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

        if (clickCount >= 2)
        {
            Debug.Log($"Save at {transform.position}");
            currentCheckPointPosition = transform.position;
            player.gameObject.GetComponentInParent<CharacterData>().SaveData();
            clickCount = 0;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            Debug.Log($"Save at: {transform.position}");
            currentCheckPointPosition = transform.position;
            other.gameObject.GetComponentInParent<CharacterData>().SaveData();
        }
    }

    public Vector3 GetSavePosition()
    {
        return currentCheckPointPosition;
    }
}
