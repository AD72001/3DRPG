using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    [SerializeField] private Button loadBtn;
    [SerializeField] private AudioClip clickSound;

    private void Start() {
        if (!File.Exists(Application.persistentDataPath + "/position.sav"))
        {
            loadBtn.interactable = false;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transition.instance.PlayTransition();
        }
    }

    public void SetClickSound()
    {
        AudioManager.instance.PlaySound(clickSound);
    }
    
    public void NewGame()
    {
        LoadStatus.LoadGame = false;
        StartCoroutine(LoadNewGame());
    }

    public void LoadGame()
    {
        LoadStatus.LoadGame = true;
        StartCoroutine(LoadNewGame());
    }

    public void Option()
    {

    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator LoadNewGame()
    {
        Transition.instance.PlayTransition();

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(1);
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
}
