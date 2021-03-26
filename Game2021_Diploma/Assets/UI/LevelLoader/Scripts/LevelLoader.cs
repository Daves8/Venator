using System.Threading;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class LevelLoader : MonoBehaviour
{    
    public GameObject loadingScreen;
    public Slider slider;
    public Animator transition;
    public GameObject loadingScreenImage;

    public void Start()
    {
        transition.SetTrigger("End");
        if(SceneManager.GetActiveScene().name == "MainMenu")Cursor.lockState = CursorLockMode.None;
        
    }

    public void ContinueGame()
    {
        Dictionary<string, string> saves = ScrollView.GetSaves();
        string last = saves.Values.Last();
        DataHolder.savePath = last;
        Debug.Log(DataHolder.savePath);
        //Player player = new Player();
        //player.LoadPlayer(last);
    }

    public void PlayScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously (string sceneName)
    {        
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            //Debug.Log(progress);
            slider.value = progress;
            yield return null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUITGAME");
        Application.Quit();
    }
}
