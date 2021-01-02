using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class UICardManager : MonoBehaviour
{
    public UICard[] cards;
    public CurrentLevelData currentLevelData;
    public SharedInt disappearEntityIndex;
    public EventListenerDelegateResponse levelLoadedResponse;
    public EventListenerDelegateResponse lightsTurnedOnResponse;
    public EventListenerDelegateResponse reappearEntityResponse;
    [HideInInspector]
    public List<int> randomOrder;

    private void OnEnable()
    {
        levelLoadedResponse.OnEnable();
        lightsTurnedOnResponse.OnEnable();
        reappearEntityResponse.OnEnable();
    }

    private void OnDisable()
    {
        levelLoadedResponse.OnDisable();
        lightsTurnedOnResponse.OnDisable();
        reappearEntityResponse.OnDisable();
    }
    private void Start()
    {
        levelLoadedResponse.response = SetCardsData;
        lightsTurnedOnResponse.response = CardsGoTarget;
        reappearEntityResponse.response = CardsGoStart;
    }
    private void Awake()
    {
        randomOrder = new List<int>(cards.Length);
        ResetRandom();
    }

    void CardsGoTarget()
    {
        foreach (var card in cards)
        {
            card.GoTargetPosition();
        }
    }

    void CardsGoStart()
    {
        Tween _cardsGoStart = null;

        foreach (var card in cards)
        {
            _cardsGoStart = card.GoStartPosition();
        }

        _cardsGoStart.OnComplete(NextEntity);
    }
    void NextEntity()
    {
        if (disappearEntityIndex.value < currentLevelData.levelData.disappearingEntities.Length - 1)
        {
            disappearEntityIndex.value++;

            ResetRandom();
            SetCardsData();
            CardsGoTarget();
        }
        else
        {
            Debug.Log("Answered");
        }
    }
    void SetCardsData()
    {
        var _disapperingEntityData = currentLevelData.levelData.disappearingEntities[disappearEntityIndex.value];

        var _randomTrue = GiveRandomIndex();

        cards[_randomTrue].SetData(true, _disapperingEntityData.name, _disapperingEntityData.sprite);

        for (int i = 0; i < cards.Length - 1; i++)
        {
            var _randomWrong = GiveRandomIndex();
            cards[_randomWrong].SetData(false, string.Empty, _disapperingEntityData.wrongAnswerSprites[i]);
        }
    }
    int GiveRandomIndex()
    {
        var _randomIndex = Random.Range(0, randomOrder.Count);
        var _randomValue = randomOrder[_randomIndex];

        randomOrder.RemoveAt(_randomIndex);

        return _randomValue;
    }
    void ResetRandom()
    {
        randomOrder.Clear();

        for (int i = 0; i < cards.Length; i++)
        {
            randomOrder.Add(i);
        }
    }
}