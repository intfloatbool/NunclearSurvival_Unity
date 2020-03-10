using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneLoader : UnitySingletonBase<SceneLoader>
{
    [SerializeField] private float _delayAfterLoading = 2f;
    public event Action<SceneType> OnSceneBeginDownloading = (sceneType) => { };
    public event Action<SceneType> OnSceneFinishDownloading = (sceneType) => { };

    private Coroutine _currentSceneLoadingCoroutine;
    private SceneType _currentScene;
    public float CurrentProgress { get; private set; }
    protected override SceneLoader GetInstance() => this;

    public void LoadScene(SceneType sceneType)
    {
        if (_currentSceneLoadingCoroutine == null)
        {
            _currentSceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(sceneType));
        }
        else
        {
            Debug.LogError("Some scene still in downloading!");
        }
    }

    private IEnumerator LoadSceneCoroutine(SceneType sceneType)
    {
        OnSceneBeginDownloading(sceneType);
        _currentScene = sceneType;
        var sceneIndex = (int)sceneType;
        var sceneAsyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        CurrentProgress = 0;
        while (!sceneAsyncLoad.isDone)
        {
            CurrentProgress = sceneAsyncLoad.progress;
            yield return null;
        }
        CurrentProgress = 1f;
        yield return new WaitForSeconds(_delayAfterLoading);
        OnSceneFinishDownloading(sceneType);
        _currentSceneLoadingCoroutine = null;
    }
}
