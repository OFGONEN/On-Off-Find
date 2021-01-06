using UnityEngine;
using FFStudio;
using UnityEngine.UI;
using DG.Tweening;

public class UICard : UIEntity
{
    public CustomCurrentLevelData currentLevelData;
    public GameEvent correctAnswerSoundEvent;
    public StringGameEvent reappearEntityEvent;
    public UIImage entityImage;
    public Image cardRenderer;
    public Button cardButton;

    [HideInInspector]
    public string disappearingEntityName;
    [HideInInspector]
    public bool isCorrect;
    public void OnChoose()
    {
        if (isCorrect)
        {
            ElephantSDK.Elephant.LevelCompleted(currentLevelData.currentLevel);
            correctAnswerSoundEvent.Raise();

            cardRenderer.DOColor(Color.green, 0.5f).OnComplete(
                () =>
                {
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

        isCorrect = correct;
        disappearingEntityName = entityName;
        entityImage.SetSprite(entitySprite);
    }
}
