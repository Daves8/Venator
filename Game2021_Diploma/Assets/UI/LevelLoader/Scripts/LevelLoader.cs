using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Не забудь добавить LevelLoader во все сцены, куда переходишь
 //       transition.SetTrigger("Start");
  //      yield return new WaitForSeconds(1);

public class LevelLoader : MonoBehaviour
{
    //public Animator transition;
    public GameObject loadingScreen;
    public Slider slider;
    public Animator transition;

    public void PlayGame(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Debug.Log(progress);
            slider.value = progress;
            yield return null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
