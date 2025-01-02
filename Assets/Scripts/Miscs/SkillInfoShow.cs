using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoShow : MonoBehaviour
{
    public static SkillInfoShow instance {get; private set; }
    private GameObject player;
    [SerializeField] private TMP_Text skill_text;
    [SerializeField] private TMP_Text skill_name;

    private void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetText(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: 
                skill_text.text = player.GetComponent<Skill_1>().GetDescription();
                skill_name.text = player.GetComponent<Skill_1>().GetName();
                break;
            case 2:
                skill_text.text = player.GetComponent<Skill_2>().GetDescription();
                skill_name.text = player.GetComponent<Skill_2>().GetName();
                break;
            case 3:
                skill_text.text = player.GetComponent<Skill_3>().GetDescription();
                skill_name.text = player.GetComponent<Skill_3>().GetName();
                break;
            case 4:
                skill_text.text = player.GetComponent<Skill_4>().GetDescription();
                skill_name.text = player.GetComponent<Skill_4>().GetName();
                break;
            default: break;
        }
    }
}
