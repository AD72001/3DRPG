using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button quitBtn;

    private void Awake()
    {
        returnBtn.onClick.AddListener(Return);
        quitBtn.onClick.AddListener(Quit);
    }

    private void OnEnable() {
        Time.timeScale = 0;
        PauseMenuUI.isPausing = true;
    }

    private void OnDisable() {
        Time.timeScale = 1;
        PauseMenuUI.isPausing = false;
    }

    public void Return()
    {
        Time.timeScale = 1;
        PauseMenuUI.isPausing = false;
        StartCoroutine(ReturnToIntro());
    }

    public void Quit()
    {
        Time.timeScale = 1;
        PauseMenuUI.isPausing = false;
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        Transition.instance.PlayTransition();

        yield return new WaitForSeconds(1);

        Application.Quit(); // Quits the application

        # if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode(); // Quit Playmode
        # endif
    }

    IEnumerator ReturnToIntro()
    {
        Transition.instance.PlayTransition();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }
}
