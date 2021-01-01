using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region UIObjects
    public UIEntity backgroundUI;
    public UISpriteImage headerImage;
    public UISpriteImage footImage;
    public UIEntity levelInfoUI;
    public TextMeshProUGUI levelInfoText;
    public UIEntity countDownUI;
    public Image lightImage;
    public TextMeshProUGUI countDownText;
    #endregion

    #region EventListeners
    public EventListenerDelegateResponse tapInputListener;
    #endregion

    #region Events
    public GameEvent startLevelEvent;
    #endregion

    public UISpriteAlbum headerImageAlbum;
    public UISpriteAlbum rootImageAlbum;
    public UISpriteAlbum lightImageImageAlbum;
    public CurrentLevelData currentLevel;

    private void OnEnable()
    {
        tapInputListener.OnEnable();
    }

    private void OnDisable()
    {
        tapInputListener.OnDisable();
    }

    private void Start()
    {
        SetLevelData();

        tapInputListener.response = StartLevel;
    }

    void StartLevel()
    {
        tapInputListener.response = EmptyMethod;

        levelInfoUI.GoTargetPosition();
        countDownUI.GoTargetPosition();

        headerImage.GoTargetPosition();
        footImage.GoTargetPosition();

        backgroundUI.GoTargetPosition();

        startLevelEvent.Raise();
    }
    void EmptyMethod()
    {

    }

    void SetLevelData()
    {
        levelInfoText.text = "Level " + currentLevel.currentLevel;

        countDownText.text = (currentLevel.levelData.countdownDuration / 10).ToString()
            + (currentLevel.levelData.countdownDuration % 10).ToString();
    }
}