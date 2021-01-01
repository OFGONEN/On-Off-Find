using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;
using DG.Tweening;

public class UICard : UIEntity
{
    public StringGameEvent reappearEntityEvent;
    public EventListenerDelegateResponse reapperEntityResponse;
    public UIImage entityImage;
    public Image cardRenderer;
    public Button cardButton;

    [HideInInspector]
    public string disappearingEntityName;
    [HideInInspector]
    public bool isCorrect;

    private void OnEnable()
    {
        reapperEntityResponse.OnEnable();
    }
    private void OnDisable()
    {
        reapperEntityResponse.OnDisable();
    }

    public override void Start()
    {
        base.Start();

        reapperEntityResponse.response = () =>
        {
            if (!isCorrect)
                cardButton.interactable = false;
        };
    }
    public void OnChoose()
    {
        if (isCorrect)
        {
            cardRenderer.DOColor(Color.green, 0.5f).OnComplete(
                () =>
                {
                    reappearEntityEvent.value = disappearingEntityName;
                    reappearEntityEvent.Raise();
                    GoStartPosition();
                }
            );
        }
        else
        {
            cardRenderer.DOColor(Color.red, 0.5f).OnComplete(GoStartPosition);
        }

        cardButton.interactable = false;
    }
    public void SetData(bool correct, string entityName, Sprite entitySprite)
    {
        cardRenderer.color = Color.white;
        cardButton.interactable = true;

        isCorrect = correct;
        disappearingEntityName = entityName;
        entityImage.SetSprite(entitySprite);
    }
}
