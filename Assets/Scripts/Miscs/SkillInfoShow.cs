using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoShow : MonoBehaviour
{
    public static SkillInfoShow instance {get; private set; }
    private GameObject player;
    [SerializeField] private TMP_Text skill_text;

    private void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetPosition(Image skillButton)
    {
        RectTransform rt = GetComponent<RectTransform>();

        transform.position = skillButton.transform.position + new Vector3(rt.sizeDelta[0], rt.sizeDelta[1], 0);
    }

    public void SetText(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: 
                skill_text.text = player.GetComponent<Skill_1>().GetDescription();
                break;
            case 2:
                skill_text.text = player.GetComponent<Skill_2>().GetDescription();
                break;
            case 3:
                skill_text.text = player.GetComponent<Skill_3>().GetDescription();
                break;
            case 4:
                skill_text.text = player.GetComponent<Skill_4>().GetDescription();
                break;
            default: break;
        }
    }
}
