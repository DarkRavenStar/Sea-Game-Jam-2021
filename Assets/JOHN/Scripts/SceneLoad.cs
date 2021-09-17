using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.DemiLib;
using DG.Tweening;


public class SceneLoad : BaseUI
{
    [SerializeField]
    private LoadingScreen loadingScreen;
    [SerializeField]
    private CanvasGroup mainCanvas;

    private AsyncOperation operation;
    private LoadSceneMode loadSceneMode;

    private void Awake()
    {
        DOTween.Init();
        DontDestroyOnLoad(gameObject);
        loadSceneMode = LoadSceneMode.Single;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        PlayAnimation(mainCanvas, 1, 1f, (() => {
            BeginLoadScene(sceneName);
        }));
    }

    private void BeginLoadScene(string sceneName)
    {
        loadingScreen.UpdateProgress(0);//reset
        StartCoroutine(BeginLoad(sceneName));
    }

    private IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!operation.isDone)
        {
            loadingScreen.UpdateProgress(operation.progress);
            yield return null;
        }

        loadingScreen.UpdateProgress(operation.progress);
        operation = null;
        mainCanvas.DOFade(0, 0.3f);
    }

}
