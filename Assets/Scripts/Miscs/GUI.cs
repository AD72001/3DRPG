using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{   
    private GameObject player;
    private GameObject enemy;

    [SerializeField] private Image playerHP;
    [SerializeField] private TMP_Text playerHP_text;

    [Header("Skill UI")]
    [SerializeField] private Image skill_1;
    [SerializeField] private Image skill_2;
    [SerializeField] private Image skill_3;
    [SerializeField] private Image skill_4;

    [SerializeField] private GameObject enemyUI;
    [SerializeField] private Image enemyHP;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;
    }

    void Update()
    {
        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;
        playerHP_text.text = $"{(int)player.GetComponent<HP>().currentHP} / {player.GetComponent<HP>().startingHP}";

        // Skills CD
        skill_1.fillAmount = (player.GetComponent<Skills>().first_CD - player.GetComponent<Skills>().first_timer) 
            / player.GetComponent<Skills>().first_CD;
        skill_2.fillAmount = (player.GetComponent<Skills>().second_CD - player.GetComponent<Skills>().second_timer) 
            / player.GetComponent<Skills>().second_CD;        
        skill_3.fillAmount = (player.GetComponent<Skills>().third_CD - player.GetComponent<Skills>().third_timer) 
            / player.GetComponent<Skills>().third_CD;
        skill_4.fillAmount = (player.GetComponent<Skills>().fourth_CD - player.GetComponent<Skills>().fourth_timer) 
            / player.GetComponent<Skills>().fourth_CD;

        enemy = player.GetComponent<CharacterCombat>().opponent;

        if (enemy!=null)
        {
            enemyUI.SetActive(true);
            enemyHP.fillAmount = enemy.GetComponent<HP>().currentHP / enemy.GetComponent<HP>().startingHP;
        }
        else enemyUI.SetActive(false);
    }
}
