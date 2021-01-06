using UnityEngine;
using FFStudio;
using UnityEngine.UI;
using DG.Tweening;

public class UICard : UIEntity
{
    public CustomCurrentLevelData currentLevelData;
    public EventListenerDelegateResponse entityReappearedResponse;
    public GameEvent correctAnswerSoundEvent;
    public StringGameEvent reappearEntityEvent;
    public UIImage entityImage;
    public Image cardRenderer;
    public Button cardButton;

    [HideInInspector]
    public string disappearingEntityName;
    [HideInInspector]
    public bool isCorrect;

    private void OnEnable()
    {
        entityReappearedResponse.OnEnable();
    }
    private void OnDisable()
    {

        entityReappearedResponse.OnDisable();
    }
    public override void Start()
    {
        base.Start();
        entityReappearedResponse.response = ChoosedCorrectCard;
    }
    public void OnChoose()
    {
        if (isCorrect)
        {
            ElephantSDK.Elephant.LevelCompleted(currentLevelData.currentLevel);
            correctAnswerSoundEvent.Raise();

            cardRenderer.DOColor(Color.green, 0.5f).OnComplete(
                () =>
                {
                    entityReappearedResponse.response = EmptyMethod;
                    reappearEntityEvent.value = disappearingEntityName;
                    reappearEntityEvent.Raise();
                }
            );
        }
        else
        {
            cardRenderer.DOColor(Color.red, 0.5f).OnComplete(() => GoStartPosition());
            ElephantSDK.Elephant.LevelFailed(currentLevelData.currentLevel);
        }

        cardButton.interactable = false;
    }
    public void SetData(bool correct, string entityName, Sprite entitySprite)
    {
        cardRenderer.color = Color.white;
        cardButton.interactable = true;

        entityReappearedResponse.response = ChoosedCorrectCard;

        isCorrect = correct;
        disappearingEntityName = entityName;
        entityImage.SetSprite(entitySprite);
    }

    void ChoosedCorrectCard()
    {
        cardButton.interactable = false;
    }
    void EmptyMethod()
    {

    }
}
