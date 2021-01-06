using System.Collections;
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

        if (countDownTick == currentLevel.levelData.countdownDuration / 2)
        {
            countDownTickResponse.OnDisable();

            tutorialEndCheck = EmptyMethod;
            leftArrowChecked = false;
            rightArrowChecked = false;
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

        var _sequence = DOTween.Sequence();
        _sequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.one, 0.4f));
        _sequence.SetLoops(3, LoopType.Restart).OnComplete(() => messageText.rectTransform.DOScale(Vector3.zero, 0.4f).OnComplete(ChooseCorrectOne));
    }
    void ChooseCorrectOne()
    {
        messageText.text = "Choose what is missing!";
        messageText.rectTransform.position = chooseTextTargetTransform.position;

        chooseSequence = DOTween.Sequence();
        chooseSequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        chooseSequence.Append(messageText.rectTransform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.4f));
        chooseSequence.SetLoops(-1, LoopType.Restart);
    }
    void MissingText()
    {
        Debug.Log(messageText.rectTransform.localScale);
        var _sequence = DOTween.Sequence();
        _sequence.Append(messageText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f));
        _sequence.Append(messageText.rectTransform.DOScale(Vector3.one, 0.4f));
        _sequence.SetLoops(6, LoopType.Restart).OnComplete(RememberText);
    }
    void RememberText()
    {
        messageText.text = "Try to remember!!";

        var _sequenceRemember = DOTween.Sequence();
        _sequenceRemember.Append(messageText.rectTransform.DOScale(Vector3.one, 0.5f));
        _sequenceRemember.AppendInterval(1f);
        _sequenceRemember.Append(messageText.rectTransform.DOScale(Vector3.zero, 0.25f));
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
