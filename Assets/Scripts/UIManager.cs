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
    public EventListenerDelegateResponse levelCompleted;
    public EventListenerDelegateResponse startLevelListener;
    public EventListenerDelegateResponse countDownTickListener;
    public EventListenerDelegateResponse countDownEndListener;
    public EventListenerDelegateResponse lightsTurnedOnListener;
    #endregion

    #region Events
    public GameEvent startLevelEvent;
    public GameEvent confettiStart;
    public GameEvent confettiEnd;
    public GameEvent nextLevelEvent;
    public IntRoutineGameEvent countDownEvent;
    #endregion

    public UISpriteAlbum headerImageAlbum;
    public UISpriteAlbum footImageAlbum;
    public UISpriteAlbum lightImageImageAlbum;
    public CurrentLevelData currentLevel;

    private void OnEnable()
    {
        tapInputListener.OnEnable();
        countDownTickListener.OnEnable();
        countDownEndListener.OnEnable();
        lightsTurnedOnListener.OnEnable();
        levelCompleted.OnEnable();
        startLevelListener.OnEnable();
    }

    private void OnDisable()
    {
        tapInputListener.OnDisable();
        countDownTickListener.OnDisable();
        countDownEndListener.OnDisable();
        lightsTurnedOnListener.OnDisable();
        levelCompleted.OnDisable();
        startLevelListener.OnDisable();
    }

    private void Start()
    {
        tapInputListener.response = StartLevel;
        countDownTickListener.response = CountDownTickResponse;
        countDownEndListener.response = CountDownEndResponse;
        lightsTurnedOnListener.response = () => lightImage.sprite = lightImageImageAlbum.GiveSprite();
        levelCompleted.response = LevelCompleted;
        startLevelListener.response = EmptyMethod;
    }
    void StartLevel()
    {
        SetLevelData();

        startLevelListener.response = EmptyMethod;
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
    void LevelCompleted()
    {
        PlayerPrefs.SetInt("Level", currentLevel.currentLevel + 1);

        levelInfoUI.GoStartPosition();
        countDownUI.GoStartPosition();
        backgroundUI.GoStartPosition();

        headerImage.SetSprite(headerImageAlbum.GiveSprite());
        footImage.SetSprite(footImageAlbum.GiveSprite());

        headerImage.GoStartPosition();
        footImage.GoStartPosition().onComplete = () =>
        {
            confettiStart.Raise();
            tapInputListener.response = NextLevel;
        };
    }
    void NextLevel()
    {
        confettiEnd.Raise();
        nextLevelEvent.Raise();

        headerImage.GoTargetPosition();
        footImage.GoTargetPosition().onComplete = () =>
        {
            headerImage.SetSprite(headerImageAlbum.GiveSprite());
            footImage.SetSprite(footImageAlbum.GiveSprite());

        };

        tapInputListener.response = EmptyMethod;
        startLevelListener.response = StartLevel;
    }
    void EmptyMethod()
    {

    }
}