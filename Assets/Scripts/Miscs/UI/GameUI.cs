using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{   
    private GameObject player;
    private GameObject enemy;

    [Header("Player UI")]
    [SerializeField] private Image playerHP;
    [SerializeField] private TMP_Text playerHP_text;
    [SerializeField] private Image playerEnergy;
    [SerializeField] private TMP_Text playerEnergy_text;
    [SerializeField] private TMP_Text playerLevel_text;
    [SerializeField] private Image playerExp;
    [SerializeField] private TMP_Text playerExp_text;
    [SerializeField] private TMP_Text playerCash_text;

    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject tutorialScreen;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject upgradeUI;

    [Header("Stats UI")]
    [SerializeField] private TMP_Text player_Str;
    [SerializeField] private TMP_Text player_Def;
    [SerializeField] private TMP_Text player_Vit;
    [SerializeField] private TMP_Text player_Int;

    [Header("Skill UI")]
    [SerializeField] private Image skill_1;
    [SerializeField] private Image skill_2;
    [SerializeField] private Image skill_3;
    [SerializeField] private Image skill_4;
    [SerializeField] private GameObject skillPanel;
    
    [Header("Skill Key")]
    [SerializeField] private TMP_Text key_1;
    [SerializeField] private TMP_Text key_2;
    [SerializeField] private TMP_Text key_3;
    [SerializeField] private TMP_Text key_4;

    [Header("Skill Level")]
    [SerializeField] private TMP_Text skill_level_1;
    [SerializeField] private TMP_Text skill_level_2;
    [SerializeField] private TMP_Text skill_level_3;
    [SerializeField] private TMP_Text skill_level_4;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private GameObject itemInfoUI;
    [SerializeField] private GameObject merchantUI;

    [Header("Enemy UI")]
    [SerializeField] private GameObject enemyUI;
    [SerializeField] private TMP_Text enemyInfo;
    [SerializeField] private Image enemyHP;

    [Header("Click Effect")]
    [SerializeField] private GameObject clickEffect;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip clickSound_2;

    [Header("Audio")]
    [SerializeField] private AudioClip victorySound;

    public static GameUI instance {get; private set; }

    private void Awake() {
        if (instance == null) instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;

        inventoryUI.SetActive(true);
        equipmentUI.SetActive(true);

        inventoryUI.SetActive(false);
        equipmentUI.SetActive(false);

        SetSkillKeyUI();

        if (!LoadStatus.LoadGame)
        {
            tutorialScreen.SetActive(true);
        }
    }

    void LateUpdate()
    {

        // HP and Energy UI
        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;
        playerHP_text.text = $"{(int)Math.Ceiling(player.GetComponent<HP>().currentHP)} / {player.GetComponent<HP>().startingHP}";

        playerEnergy.fillAmount = player.GetComponent<Energy>().GetEnergy() / player.GetComponent<Energy>().GetEnergyMax();
        playerEnergy_text.text = $"{(int)player.GetComponent<Energy>().GetEnergy()} / {player.GetComponent<Energy>().GetEnergyMax()}";

        // Stat UI
        player_Str.text = "STR: " + player.GetComponent<Stat>().GetStr();
        player_Def.text = "DEF: " + player.GetComponent<Stat>().GetDef();
        player_Vit.text = "VIT: " + player.GetComponent<Stat>().GetVit();
        player_Int.text = "INT: " + player.GetComponent<Stat>().GetInt();

        if (Input.GetKeyDown(KeyCode.N))
        {
            player.GetComponent<Skill_1>().level += 1;
            UpdateSkillLevel();
        }

        // Inventory UI
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.activeSelf)
                inventoryUI.SetActive(false);
            else
                inventoryUI.SetActive(true);
        }

        foreach (Transform child in merchantUI.transform)
        {
            if (child.gameObject.activeSelf)
                inventoryUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (equipmentUI.activeSelf)
                equipmentUI.SetActive(false);
            else
                equipmentUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (equipmentUI.activeSelf || inventoryUI.activeSelf || 
                itemInfoUI.activeSelf || IsShopping())
            {
                equipmentUI.SetActive(false);
                inventoryUI.SetActive(false);
                itemInfoUI.SetActive(false);
                
                foreach (Transform child in merchantUI.transform)
                    child.gameObject.SetActive(false);

                MerchantInterface.isShopping = false;
                Time.timeScale = 1;
                PauseMenuUI.isPausing = false;
            }
            else {
                pauseMenuUi.SetActive(!pauseMenuUi.activeSelf);
            }
        }

        playerCash_text.text = player.GetComponent<CharacterInventory>().GetCash().ToString();;

        // Exp UI
        playerLevel_text.text = $"{player.GetComponent<Stat>().level}";
        playerExp.fillAmount = (float) player.GetComponent<Stat>().totalExp / player.GetComponent<Stat>().threshold;
        playerExp_text.text = $"{player.GetComponent<Stat>().totalExp} / {player.GetComponent<Stat>().threshold}";

        // Skills CD
        skill_1.fillAmount = (player.GetComponent<Skill_1>().CD - player.GetComponent<Skill_1>().timer) 
            / player.GetComponent<Skill_1>().CD;
        skill_2.fillAmount = (player.GetComponent<Skill_2>().CD - player.GetComponent<Skill_2>().timer) 
            / player.GetComponent<Skill_2>().CD;
        skill_3.fillAmount = (player.GetComponent<Skill_3>().CD - player.GetComponent<Skill_3>().timer) 
            / player.GetComponent<Skill_3>().CD;
        skill_4.fillAmount = (player.GetComponent<Skill_4>().CD - player.GetComponent<Skill_4>().timer) 
            / player.GetComponent<Skill_4>().CD;


        enemy = player.GetComponent<CharacterCombat>().opponent;

        if (enemy!=null)
        {
            enemyUI.SetActive(true);
            enemyHP.fillAmount = enemy.GetComponent<HP>().currentHP / enemy.GetComponent<HP>().startingHP;
            if (enemy.GetComponent<Enemy>())
                enemyInfo.text = $"{enemy.GetComponent<Enemy>().enemyName} - {enemy.GetComponent<Stat>().level}";
            else
            {
                enemyInfo.text = $"{enemy.GetComponent<WizardEnemy>().enemyName} - {enemy.GetComponent<Stat>().level}";
            }
        }
        else enemyUI.SetActive(false);
    }

    public void ShowSkillInfo(int skillIndex)
    {
        skillPanel.SetActive(true);
        SkillInfoShow.instance.SetText(skillIndex);
    }

    public void SetUpgradeUI(bool status)
    {
        upgradeUI.SetActive(status);
    }

    public void UpdateSkillLevel()
    {
        skill_level_1.text = player.GetComponent<Skill_1>().level.ToString();
        skill_level_2.text = player.GetComponent<Skill_2>().level.ToString();
        skill_level_3.text = player.GetComponent<Skill_3>().level.ToString();
        skill_level_4.text = player.GetComponent<Skill_4>().level.ToString();
    }

    public void ActivateVictoryScreen()
    {
        victoryUI.SetActive(true);
        AudioManager.instance.PlaySound(victorySound);
    }

    public void ActiveDefeatScreen(bool status)
    {
        defeatScreen.SetActive(status);
    }

    public void EndDefeatScreen()
    {
        defeatScreen.GetComponent<Animator>().SetBool("End", true);
    }

    private bool IsShopping()
    {
        foreach (Transform child in merchantUI.transform)
        {
            if (child.gameObject.activeSelf)
                return true;
        }

        return false;
    }

    public void ActivateClickEffect()
    {
        clickEffect.SetActive(true);
    }

    public void DeactivateClickEffect()
    {
        clickEffect.SetActive(false);
    }

    public void SetClickEffectPosition(Vector3 position)
    {
        clickEffect.transform.position = position;
    }

    public void SetClickSound()
    {
        AudioManager.instance.PlaySound(clickSound);
    }

    public void SetClickSound_2()
    {
        AudioManager.instance.PlaySound(clickSound_2);
    }

    public void SetSkillKeyUI()
    {
        key_1.text = player.GetComponent<Skill_1>().keyCode.ToString();
        key_2.text = player.GetComponent<Skill_2>().keyCode.ToString();
        key_3.text = player.GetComponent<Skill_3>().keyCode.ToString();
        key_4.text = player.GetComponent<Skill_4>().keyCode.ToString();
    }

    public void ActivateUI(GameObject activateUI)
    {
        activateUI.SetActive(true);
    }

    public void Deactivate(GameObject deactivateUI)
    {
        deactivateUI.SetActive(false);
    }
}
