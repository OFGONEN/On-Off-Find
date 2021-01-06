﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using DG.Tweening;

public class UISwipeTutorial : MonoBehaviour
{
    public int[] tutorialLevels;
    public CameraDrag cameraDrag;
    public CustomCurrentLevelData currentLevel;

    public UIEntity leftArrow;
    public Image leftArrowImage;
    public UIEntity rightArrow;
    public Image rightArrowImage;
    public TextMeshProUGUI messageText;
    public RectTransform chooseTextTargetTransform;


    #region EventListeners
    public EventListenerDelegateResponse cameraMoveEndResponse;
    public EventListenerDelegateResponse countDownTickResponse;
    public EventListenerDelegateResponse lightsTurnedOnResponse;
    public EventListenerDelegateResponse entityReappearedResponse;
    #endregion

    delegate void VoidDelegate();
    VoidDelegate leftArrowCheck;
    VoidDelegate rightArrowCheck;
    VoidDelegate tutorialEndCheck;

    bool leftArrowChecked;
    bool rightArrowChecked;
    bool tutorialActive;
    int countDownTick = 0;
    Sequence chooseSequence;

    float[] clampValues;

    private void OnEnable()
    {
        cameraMoveEndResponse.OnEnable();
        countDownTickResponse.OnEnable();
        lightsTurnedOnResponse.OnEnable();
        entityReappearedResponse.OnEnable();
    }

    private void OnDisable()
    {
        cameraMoveEndResponse.OnDisable();
        countDownTickResponse.OnDisable();
        lightsTurnedOnResponse.OnDisable();
        entityReappearedResponse.OnDisable();
    }
    private void Awake()
    {
        leftArrowCheck = EmptyMethod;
        rightArrowCheck = EmptyMethod;
        tutorialEndCheck = EmptyMethod;

        clampValues = new float[2];
    }
    private void Start()
    {
        cameraMoveEndResponse.response = CanStartLevel;
        countDownTickResponse.response = EmptyMethod;
        lightsTurnedOnResponse.response = EmptyMethod;
        entityReappearedResponse.response = EmptyMethod;
    }

    private void Update()
    {
        leftArrowCheck();
        rightArrowCheck();
        tutorialEndCheck();
    }
    void CountDownTickResponse()
    {
        countDownTick++;

        if (tutorialActive && countDownTick == currentLevel.levelData.countdownDuration / 2)
        {
            countDownTickResponse.OnDisable();

            tutorialEndCheck = EmptyMethod;
            leftArrowChecked = false;
            rightArrowChecked = false;
            tutorialActive = false;
            EndSwipeTutorial();
        }
    }

    void StartLevel()
    {

        leftArrowImage.color = Color.white;
        rightArrowImage.color = Color.white;

        messageText.gameObject.SetActive(true);

        messageText.text = "Swipe to Look Around";

        leftArrow.GoTargetPosition();
        rightArrow.GoTargetPosition();

        messageText.rectTransform.DOScale(Vector3.one, 0.25f);

        leftArrowChecked = false;
        rightArrowChecked = false;
        tutorialActive = true;
        countDownTick = 0;

        countDownTickResponse.response = CountDownTickResponse;
        lightsTurnedOnResponse.response = LookAroundText;
        entityReappearedResponse.response = EntityReappeared;


        leftArrowCheck = LeftArrowCheckMethod;
        rightArrowCheck = RightArrowCheckMethod;
        tutorialEndCheck = EndSwipeTutorialCheck;
    }
    void EntityReappeared()
    {
        var _sequence = DOTween.Sequence();

        _sequence.Append(messageText.DOColor(Color.green, 0.5f));
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.zero, 0.4f))
        .OnComplete(() => messageText.gameObject.SetActive(false));
    }
    void LookAroundText()
    {
        messageText.text = "You can still Look Around";
        messageText.gameObject.SetActive(true);
        messageText.rectTransform.DOScale(Vector3.one, 0.4f).OnComplete(LookAroundHover);
    }
    void LookAroundHover()
    {
        var _sequence = DOTween.Sequence();
        _sequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.one, 0.4f));
        _sequence.SetLoops(3, LoopType.Yoyo).OnComplete(() => messageText.rectTransform.DOScale(Vector3.zero, 0.4f).OnComplete(ChooseCorrectOne));
    }
    void ChooseCorrectOne()
    {
        messageText.text = "Which item went missing?";
        messageText.rectTransform.position = chooseTextTargetTransform.position;
        messageText.rectTransform.DOScale(Vector3.one, 0.4f).OnComplete(ChooseMissinHover);
    }
    void ChooseMissinHover()
    {
        chooseSequence = DOTween.Sequence();
        chooseSequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        chooseSequence.Append(messageText.rectTransform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.4f));
        chooseSequence.SetLoops(-1, LoopType.Yoyo);
    }
    void MissingText()
    {
        var _sequence = DOTween.Sequence();
        _sequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.one, 0.4f));
        _sequence.SetLoops(6, LoopType.Yoyo).OnComplete(RememberText);
    }
    void RememberText()
    {
        messageText.text = "Try to remember!!";

        var _sequenceRemember = DOTween.Sequence();
        _sequenceRemember.Append(messageText.rectTransform.DOScale(Vector3.one, 0.25f));
        _sequenceRemember.AppendInterval(1.25f);
        _sequenceRemember.Append(messageText.rectTransform.DOScale(Vector3.zero, 0.15f));
        _sequenceRemember.OnComplete(() => messageText.gameObject.SetActive(false));
    }
    void EndSwipeTutorial()
    {
        var _sequence = DOTween.Sequence();
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.zero, 0.25f)).AppendInterval(0.5f);
        _sequence.AppendCallback(() => messageText.text = "Some Outlined Objects will go missing");
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.one, 0.25f));
        _sequence.OnComplete(MissingText);

        leftArrow.GoStartPosition();
        rightArrow.GoStartPosition();
    }
    void EndSwipeTutorialCheck()
    {
        if (leftArrowChecked && rightArrowChecked)
        {
            tutorialEndCheck = EmptyMethod;
            leftArrowChecked = false;
            rightArrowChecked = false;
            tutorialActive = false;
            EndSwipeTutorial();
        }
    }
    void LeftArrowCheckMethod()
    {
        clampValues[0] = cameraDrag.clampValues[0] + currentLevel.gameSettings.cameraTurnDegree * 0.90f;

        if (cameraDrag.transform.rotation.eulerAngles.y <= clampValues[0])
        {
            leftArrowCheck = EmptyMethod;
            leftArrowChecked = true;
            leftArrowImage.DOColor(Color.green, 0.5f).OnComplete(() => leftArrow.GoStartPosition());
        }
    }

    void RightArrowCheckMethod()
    {
        clampValues[1] = cameraDrag.clampValues[1] - currentLevel.gameSettings.cameraTurnDegree * 0.90f;

        if (cameraDrag.transform.rotation.eulerAngles.y >= clampValues[1])
        {
            rightArrowCheck = EmptyMethod;
            rightArrowChecked = true;
            rightArrowImage.DOColor(Color.green, 0.5f).OnComplete(() => rightArrow.GoStartPosition());
        }
    }

    void CanStartLevel()
    {
        for (int i = 0; i < tutorialLevels.Length; i++)
        {
            if (tutorialLevels[i] == currentLevel.currentLevel)
            {
                StartLevel();
                return;
            }
        }

        cameraMoveEndResponse.OnDisable();
        countDownTickResponse.OnDisable();
        lightsTurnedOnResponse.OnDisable();
        entityReappearedResponse.OnDisable();
    }
    void EmptyMethod()
    {

    }
}
