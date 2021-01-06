using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class UICountDown : UIEntity
{
    public CustomCurrentLevelData currentLevel;
    public EventListenerDelegateResponse lightsTurnedOn;
    public EventListenerDelegateResponse countDownStarts;

    public RectTransform lightIconTransform;
    public RectTransform lightIconDestination;
    [HideInInspector] public Vector3 lightIconStartPosition;
    public Color fillImageStartColor;
    [HideInInspector] public Image fillImage;

    private void OnEnable()
    {
        lightsTurnedOn.OnEnable();
        countDownStarts.OnEnable();
    }
    public override void Start()
    {
        base.Start();
        lightIconStartPosition = lightIconTransform.anchoredPosition;
        fillImageStartColor = fillImage.color;

        countDownStarts.response = CountDownStarts;
        // countDownStarts.response = () => { };
        lightsTurnedOn.response = LightsTurnedOn;
    }
    private void OnDisable()
    {
        lightsTurnedOn.OnDisable();
        countDownStarts.OnDisable();
    }

    [Button]
    void CountDownStarts()
    {
        var _duration = currentLevel.levelData.countdownDuration;

        lightIconTransform.DOAnchorPos(lightIconDestination.anchoredPosition, _duration).SetEase(Ease.Linear);
        fillImage.DOFillAmount(0, _duration).SetEase(Ease.Linear);
        // fillImage.DOColor(Color.black, _duration).SetEase(Ease.Linear);
    }

    void LightsTurnedOn()
    {
        lightIconTransform.DOAnchorPos(lightIconStartPosition, 0.2f);
        fillImage.DOFillAmount(1, 0.2f);
        // fillImage.DOColor(fillImageStartColor, 0.2f);
    }
}
