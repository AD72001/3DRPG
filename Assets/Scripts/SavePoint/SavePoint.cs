using UnityEngine;
using UnityEngine.EventSystems;

public class SavePoint : MonoBehaviour,  IPointerClickHandler
{
    private GameObject player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(pointerEventData.button);
        
        if (pointerEventData.button == PointerEventData.InputButton.Right || pointerEventData.clickCount >= 2)
        {
            player.gameObject.GetComponentInParent<CharacterData>().SaveData();
            Debug.Log($"Save at: {transform.position}");
            pointerEventData.clickCount = 0;
        }

        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            player.gameObject.GetComponentInParent<CharacterData>().SaveData();
            Debug.Log($"Save at: {transform.position}");
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("PlayerCollision"))
        {
            Debug.Log($"Save at: {transform.position}");
            other.gameObject.GetComponentInParent<CharacterData>().SaveData();
        }
    }
}
