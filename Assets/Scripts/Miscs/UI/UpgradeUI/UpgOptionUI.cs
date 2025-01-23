using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image img;
    [SerializeField] private AudioClip hoverSound;

    private void Awake() {
        img = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.SetAsLastSibling();
        AudioManager.instance.PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void FinishSelected()
    {
        transform.parent.GetComponent<UpgradeMenuUI>().EndUpgrade();
    }
}
