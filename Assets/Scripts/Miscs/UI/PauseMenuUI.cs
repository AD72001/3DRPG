using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button returnBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] public static bool isPausing;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(Resume);
        optionBtn.onClick.AddListener(Option);
        returnBtn.onClick.AddListener(Return);
        quitBtn.onClick.AddListener(Quit);

        this.gameObject.SetActive(false);

        isPausing = false;
    }

    private void OnEnable() {
        Time.timeScale = 0;
        isPausing = true;
    }

    private void OnDisable() {
        Time.timeScale = 1;
        isPausing = false;
    }

    public void Resume()
    {
        this.gameObject.SetActive(false);
    }

    public void Option() 
    {

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

    public void EffectVolume()
    {
        AudioManager.instance.ChangeEffectVolume(0.2f);
    }

    public void BGMVolume()
    {
        AudioManager.instance.ChangeBGMVolume(0.2f);
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

        SceneManager.LoadSceneAsync(0);
    }
}
