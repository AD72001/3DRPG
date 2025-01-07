using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.Convert;

public class SkillUIInterface : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameUI.instance.ShowSkillInfo(ToInt32(gameObject.name.Split("_")[1]));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillInfoShow.instance.gameObject.SetActive(false);
    }
}
