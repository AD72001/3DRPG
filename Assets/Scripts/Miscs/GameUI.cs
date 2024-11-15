using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject equipmentUI;

    [Header("Enemy UI")]
    [SerializeField] private GameObject enemyUI;
    [SerializeField] private TMP_Text enemyInfo;
    [SerializeField] private Image enemyHP;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;

        inventoryUI.SetActive(false);
        equipmentUI.SetActive(false);
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

        // Inventory UI
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.activeSelf)
                inventoryUI.SetActive(false);
            else
                inventoryUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (equipmentUI.activeSelf)
                equipmentUI.SetActive(false);
            else
                equipmentUI.SetActive(true);
        }

        // Exp UI
        playerLevel_text.text = $"{player.GetComponent<Stat>().level}";
        playerExp.fillAmount = (float) player.GetComponent<Stat>().totalExp / player.GetComponent<Stat>().threshold;
        playerExp_text.text = $"{player.GetComponent<Stat>().totalExp} / {player.GetComponent<Stat>().threshold}";

        // Skills CD
        skill_1.fillAmount = (player.GetComponent<First_Skill>().CD - player.GetComponent<First_Skill>().timer) 
            / player.GetComponent<First_Skill>().CD;
        skill_2.fillAmount = (player.GetComponent<Second_Skill>().CD - player.GetComponent<Second_Skill>().timer) 
            / player.GetComponent<Second_Skill>().CD;
        skill_3.fillAmount = (player.GetComponent<Third_Skill>().CD - player.GetComponent<Third_Skill>().timer) 
            / player.GetComponent<Third_Skill>().CD;
        skill_4.fillAmount = (player.GetComponent<Fourth_Skill>().CD - player.GetComponent<Fourth_Skill>().timer) 
            / player.GetComponent<Fourth_Skill>().CD;

        enemy = player.GetComponent<CharacterCombat>().opponent;

        if (enemy!=null)
        {
            enemyUI.SetActive(true);
            enemyHP.fillAmount = enemy.GetComponent<HP>().currentHP / enemy.GetComponent<HP>().startingHP;
            enemyInfo.text = $"{enemy.GetComponent<Enemy>().enemyName} - {enemy.GetComponent<Stat>().level}";
        }
        else enemyUI.SetActive(false);
    }
}
