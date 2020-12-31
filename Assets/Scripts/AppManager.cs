using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FFStudio;

public class AppManager : MonoBehaviour
{
    public CustomCurrentLevelData currentLevelData;
    public GameEvent startLevelEvent;
    public WaitForSeconds startLevelWaitTime;
    private void Start()
    {
        currentLevelData.currentLevel = PlayerPrefs.GetInt("Level", 1);

        currentLevelData.LoadCurrentLevelData();
        SceneManager.LoadScene(currentLevelData.levelData.sceneBuildIndex, LoadSceneMode.Additive);

        startLevelWaitTime = new WaitForSeconds(0.1f);
        StartCoroutine(SetUpLevel());
    }

    IEnumerator SetUpLevel()
    {
        yield return startLevelWaitTime;
        startLevelEvent.Raise();
    }
}
