using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG;
using FFStudio;
using DG.Tweening;

public class AppManager : MonoBehaviour
{
    public CustomCurrentLevelData currentLevelData;
    public GameEvent newLevelLoadingEvent;
    public GameEvent levelLoadedEvent;
    public GameEvent cleanUpEvent;
    public GameEvent startLevelEvent;
    public GameEvent startLevelInSameRoomEvent;
    public EventListenerDelegateResponse nextLevelListener;
    public EventListenerDelegateResponse levelLoadedListener;
    public WaitForSeconds startLevelWaitTime;
    public WaitForSeconds lightOffWaitTime;

    private void OnEnable()
    {
        nextLevelListener.OnEnable();
        levelLoadedListener.OnEnable();
    }
    private void OnDisable()
    {
        nextLevelListener.OnDisable();
        levelLoadedListener.OnDisable();
    }
    private void Start()
    {
        nextLevelListener.response = NextLevel;
        levelLoadedListener.response = EmptyMethod;

        currentLevelData.currentLevel = PlayerPrefs.GetInt("Level", 1);

        currentLevelData.LoadCurrentLevelData();
        SceneManager.LoadScene(currentLevelData.levelData.sceneBuildIndex, LoadSceneMode.Additive);

        startLevelWaitTime = new WaitForSeconds(0.1f);
        lightOffWaitTime = new WaitForSeconds(currentLevelData.gameSettings.lightTurnOffDuration + 0.1f);
        StartCoroutine(WaitRoutine(levelLoadedEvent.Raise, startLevelWaitTime));
    }
    IEnumerator WaitRoutine(TweenCallback onComplete, WaitForSeconds wait)
    {
        yield return wait;
        onComplete();
    }
    void NextLevel()
    {
        levelLoadedListener.response = EmptyMethod;

        cleanUpEvent.Raise();

        int _currentBuildIndex = currentLevelData.levelData.sceneBuildIndex;

        currentLevelData.currentLevel++;
        currentLevelData.LoadCurrentLevelData();

        if (_currentBuildIndex == currentLevelData.levelData.sceneBuildIndex)
        {
            startLevelInSameRoomEvent.Raise();
            levelLoadedEvent.Raise();
        }
        else
        {
            newLevelLoadingEvent.Raise();
            StartCoroutine(WaitRoutine(() => LoadNextLevel(_currentBuildIndex), lightOffWaitTime));
            levelLoadedListener.response = startLevelEvent.Raise;
        }
    }
    void LoadNextLevel(int unloadSceneIndex)
    {
        var _operation = SceneManager.UnloadSceneAsync(unloadSceneIndex);
        _operation.completed += UnloadedCurrentLevel;
    }
    void UnloadedCurrentLevel(AsyncOperation operation)
    {
        var _operation = SceneManager.LoadSceneAsync(currentLevelData.levelData.sceneBuildIndex, LoadSceneMode.Additive);
        _operation.completed += LoadedNextLevel;
    }

    void LoadedNextLevel(AsyncOperation operation)
    {
        StartCoroutine(WaitRoutine(levelLoadedEvent.Raise, startLevelWaitTime));
    }

    void EmptyMethod()
    {

    }
}
