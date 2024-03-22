using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] string sceneName = "Main Scene";
    [SerializeField] GameObject continueButton;
    [SerializeField] FadeScreenUI fadeScreen;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
    }


    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("Exit game...");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }

}
