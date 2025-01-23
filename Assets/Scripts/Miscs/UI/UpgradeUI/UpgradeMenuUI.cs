using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuUI : MonoBehaviour
{
    private GameObject player;

    private Skill skill_1, skill_2, skill_3, skill_4;
    private Stat stat;
    private bool selected = false;

    // Skill Upgrade UI
    [SerializeField] private GameObject skill_btn_1;
    [SerializeField] private GameObject skill_btn_2;
    [SerializeField] private GameObject skill_btn_3;
    [SerializeField] private GameObject skill_btn_4;

    // Stats Upgrade UI
    [SerializeField] private GameObject str_btn;
    [SerializeField] private GameObject def_btn;
    [SerializeField] private GameObject vit_btn;
    [SerializeField] private GameObject int_btn;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");

        stat = player.GetComponent<Stat>();

        skill_1 = player.GetComponent<Skill_1>();
        skill_2 = player.GetComponent<Skill_2>();
        skill_3 = player.GetComponent<Skill_3>();
        skill_4 = player.GetComponent<Skill_4>();

    }

    private void Start() {
        selected = false;

        CheckSkillUpgradeOption();
        StartUpgrade();
    }

    private void OnEnable() {
        selected = false;

        Time.timeScale = 0;
        PauseMenuUI.isPausing = true;

        CheckSkillUpgradeOption();
        StartUpgrade();
    }

    private void CheckSkillUpgradeOption()
    {
        if (skill_1.level >= 4)
        {
            skill_btn_1.SetActive(false);
        } 
        else skill_btn_1.SetActive(true);

        if (skill_2.level >= 4 || stat.level < 2)
        {
            skill_btn_2.SetActive(false);
        } 
        else skill_btn_2.SetActive(true);

        if (skill_3.level >= 4 || stat.level < 3)
        {
            skill_btn_3.SetActive(false);
        } 
        else skill_btn_3.SetActive(true);

        if (skill_4.level >= 4 || stat.level < 6)
        {
            skill_btn_4.SetActive(false);
        } 
        else skill_btn_4.SetActive(true);
    }

    public void SkillLevelUp(int skillIndex)
    {
        if (selected) return;

        selected = true;

        switch (skillIndex) {
            case 1:
                skill_1.level++;
                break;
            case 2:
                skill_2.level++;
                break;
            case 3:
                skill_3.level++;
                break;
            case 4:
                skill_4.level++;
                break;
            default:
                break;
        }

        GameUI.instance.UpdateSkillLevel();
    }

    public void StatLevelUp(int statIndex)
    {
        if (selected) return;

        selected = true;

        switch (statIndex) {
            case 1:
                stat.AddStr(5);
                break;
            case 2:
                stat.AddDef(10);
                break;
            case 3:
                stat.AddVit(5);
                break;
            case 4:
                stat.AddInt(8);
                break;
            default:
                break;
        }
    }

    public void StartUpgrade()
    {
        skill_btn_1.GetComponent<Button>().interactable = true;
        skill_btn_2.GetComponent<Button>().interactable = true;
        skill_btn_3.GetComponent<Button>().interactable = true;
        skill_btn_4.GetComponent<Button>().interactable = true;

        str_btn.GetComponent<Button>().interactable = true;
        def_btn.GetComponent<Button>().interactable = true;
        vit_btn.GetComponent<Button>().interactable = true;
        int_btn.GetComponent<Button>().interactable = true;
    }

    public void EndUpgrade()
    {
        
        skill_btn_1.GetComponent<Button>().interactable = false;
        skill_btn_2.GetComponent<Button>().interactable = false;
        skill_btn_3.GetComponent<Button>().interactable = false;
        skill_btn_4.GetComponent<Button>().interactable = false;

        str_btn.GetComponent<Button>().interactable = false;
        def_btn.GetComponent<Button>().interactable = false;
        vit_btn.GetComponent<Button>().interactable = false;
        int_btn.GetComponent<Button>().interactable = false;

        GetComponent<Animator>().SetTrigger("End");
    }

    public void Disable()
    {
        Time.timeScale = 1;
        PauseMenuUI.isPausing = false;
        this.gameObject.SetActive(false);
    }

}
