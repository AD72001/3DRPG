using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SkillBarUI : MonoBehaviour
{
    [Header("SkillBar")]
    [SerializeField] private GameObject skillBar;
    [SerializeField] private Rect position;
    [SerializeField] private float offset;

    // Skill Slots
    [Header("Skill Slots")]
    [SerializeField] private Rect skillPosition;
    [SerializeField] private float skillOffset;

    public GameObject[] skills;

    void OnGUI()
    {
        drawSkillBar();
        drawSkills();
    }

    void drawSkillBar()
    {
        skillBar.transform.position = new Vector3(
            Screen.width * position.x + offset*Screen.width
            , Screen.height * position.y, 0);
        skillBar.GetComponent<RectTransform>().sizeDelta = 
            new Vector3(Screen.width * position.width, Screen.height * position.height, 1);
    }

    void drawSkills()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].transform.position = new Vector3(
                Screen.width * skillPosition.x * i + skillOffset*Screen.width
                , Screen.height * skillPosition.y, 0);

            skills[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = 
                new Vector3(Screen.width * skillPosition.width, Screen.height * skillPosition.height, 1);
            skills[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = 
                new Vector3(Screen.width * skillPosition.width, Screen.height * skillPosition.height, 1);
        }
    }
}
