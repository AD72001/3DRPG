using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{   
    private GameObject player;
    private GameObject enemy;

    [SerializeField] private GameObject enemyUI;
    [SerializeField] private Image playerHP;
    [SerializeField] private Image enemyHP;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;
    }

    void Update()
    {
        playerHP.fillAmount = player.GetComponent<HP>().currentHP / player.GetComponent<HP>().startingHP;
        enemy = player.GetComponent<CharacterCombat>().opponent;

        if (enemy!=null)
        {
            enemyUI.SetActive(true);
            enemyHP.fillAmount = enemy.GetComponent<HP>().currentHP / enemy.GetComponent<HP>().startingHP;
        }
        else enemyUI.SetActive(false);
    }
}
