using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FFStudio;

public class AppManager : MonoBehaviour
{
    public CustomCurrentLevelData currentLevelData;
    public GameEvent levelLoadedEvent;
    public GameEvent cleanUpEvent;
    public GameEvent startLevelInSameRoomEvent;
    public EventListenerDelegateResponse nextLevelListener;
    public WaitForSeconds startLevelWaitTime;

    private void OnEnable()
    {
        nextLevelListener.OnEnable();
    }
    private void OnDisable()
    {
        nextLevelListener.OnDisable();
    }
    private void Start()
    {
        nextLevelListener.response = NextLevel;

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
    void NextLevel()
    {
        cleanUpEvent.Raise();

        int _currentBuildIndex = currentLevelData.currentLevel;

        currentLevelData.currentLevel++;
        currentLevelData.LoadCurrentLevelData();

        if (_currentBuildIndex == currentLevelData.levelData.sceneBuildIndex)
        {
            startLevelInSameRoomEvent.Raise();
            levelLoadedEvent.Raise();
        }
        else
        {
            //Isiklari kapamak lazim
            LoadNextLevel();
        }
    }
    void LoadNextLevel()
    {
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
