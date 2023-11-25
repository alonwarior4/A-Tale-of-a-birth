using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{

    [Header("Loading Screen Configs")]
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Slider LoadProgressSlider;

    [Header("loading screen fake extra wait time")]
    [SerializeField] float fakeWaitTime;
    float fakeTimePassProgress;

    WaitForEndOfFrame waitToEndFrame = new WaitForEndOfFrame();

    private void Start()
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadMainGame());
    }

    IEnumerator LoadMainGame()
    {
        float totalProgress = 0;
        AsyncOperation LoadMainScene = SceneManager.LoadSceneAsync("Wall", LoadSceneMode.Additive);
        StartCoroutine(CalculateFakeTimePassProgress());
        
        while (!LoadMainScene.isDone || totalProgress < 0.97f)
        {
            totalProgress = 0;
            totalProgress = (LoadMainScene.progress / 1.8f) + (fakeTimePassProgress / 2);
            LoadProgressSlider.value = totalProgress;
            yield return waitToEndFrame;
        }

        LoadingScreen.SetActive(false);



        //TapsellAdManager._Instance.ShowFirstTapsellAd();
        TapsellAdManager._Instance.RequestAndShowAd();

    }

    IEnumerator CalculateFakeTimePassProgress()
    {
        float cashFakeTime = fakeWaitTime;
        while(cashFakeTime > Mathf.Epsilon)
        {
            cashFakeTime -= Time.unscaledDeltaTime;
            fakeTimePassProgress = 1 - (cashFakeTime/fakeWaitTime);
            yield return waitToEndFrame;
        }           
    }
}
