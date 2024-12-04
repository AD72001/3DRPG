using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private GameObject player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            Debug.Log($"Save at: {transform.position}");
            other.gameObject.GetComponentInParent<CharacterData>().SaveData();
        }
    }
}
