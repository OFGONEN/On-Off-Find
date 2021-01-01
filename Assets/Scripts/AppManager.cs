using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FFStudio;

public class AppManager : MonoBehaviour
{
    public CustomCurrentLevelData currentLevelData;
    public GameEvent levelLoadedEvent;
    public EventListenerDelegateResponse loadNextLevelListener;
    public WaitForSeconds startLevelWaitTime;

    private void OnEnable()
    {
        loadNextLevelListener.OnEnable();
    }
    private void OnDisable()
    {
        loadNextLevelListener.OnDisable();
    }
    private void Start()
    {
        loadNextLevelListener.response = LoadNextLevel;

        currentLevelData.currentLevel = PlayerPrefs.GetInt("Level", 1);

        currentLevelData.LoadCurrentLevelData();
        SceneManager.LoadScene(currentLevelData.levelData.sceneBuildIndex, LoadSceneMode.Additive);

        startLevelWaitTime = new WaitForSeconds(0.1f);
        StartCoroutine(SetUpLevel());
    }

    IEnumerator SetUpLevel()
    {
        yield return startLevelWaitTime;
        levelLoadedEvent.Raise();
    }

    void LoadNextLevel()
    {
        currentLevelData.currentLevel++;
        currentLevelData.LoadCurrentLevelData();

        var _operation = SceneManager.UnloadSceneAsync(currentLevelData.levelData.sceneBuildIndex);
        _operation.completed += UnloadedCurrentLevel;
    }

    void UnloadedCurrentLevel(AsyncOperation operation)
    {
        var _operation = SceneManager.LoadSceneAsync(currentLevelData.levelData.sceneBuildIndex, LoadSceneMode.Additive);
        _operation.completed += LoadedNextLevel;
    }

    void LoadedNextLevel(AsyncOperation operation)
    {
        StartCoroutine(SetUpLevel());
    }
}
