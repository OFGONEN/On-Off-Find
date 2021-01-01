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
    public EventListenerDelegateResponse countDownTickListener;
    public EventListenerDelegateResponse countDownEndListener;
    #endregion

    #region Events
    public GameEvent startLevelEvent;
    public IntRoutineGameEvent countDownEvent;
    #endregion

    public UISpriteAlbum headerImageAlbum;
    public UISpriteAlbum rootImageAlbum;
    public UISpriteAlbum lightImageImageAlbum;
    public CurrentLevelData currentLevel;

    private void OnEnable()
    {
        tapInputListener.OnEnable();
        countDownTickListener.OnEnable();
        countDownEndListener.OnEnable();
    }

    private void OnDisable()
    {
        tapInputListener.OnDisable();
        countDownTickListener.OnDisable();
        countDownEndListener.OnDisable();
    }

    private void Start()
    {
        SetLevelData();

        tapInputListener.response = StartLevel;
        countDownTickListener.response = CountDownTickResponse;
        countDownEndListener.response = CountDownEndResponse;
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

        countDownEvent.value = currentLevel.levelData.countdownDuration;
        countDownEvent.StartRoutine(this);
    }
    void CountDownTickResponse()
    {
        countDownText.text = (countDownEvent.value / 10).ToString()
           + (countDownEvent.value % 10).ToString();
    }
    void CountDownEndResponse()
    {
        countDownText.text = "00";
        lightImage.sprite = lightImageImageAlbum.GiveSprite();
    }
    void SetLevelData()
    {
        levelInfoText.text = "Level " + currentLevel.currentLevel;

        countDownText.text = (currentLevel.levelData.countdownDuration / 10).ToString()
            + (currentLevel.levelData.countdownDuration % 10).ToString();
    }
    void EmptyMethod()
    {

    }
}