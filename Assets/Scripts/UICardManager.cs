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
    public GameEvent levelCompletedEvent;
    public EventListenerDelegateResponse levelLoadedResponse;
    public EventListenerDelegateResponse lightsTurnedOnResponse;
    public EventListenerDelegateResponse reappearEntityResponse;

    List<int> randomOrder;

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

            SetCardsData();
            CardsGoTarget();
        }
        else
        {
            levelCompletedEvent.Raise();
        }
    }
    void SetCardsData()
    {
        ResetRandom();
        var _disapperingEntityData = currentLevelData.levelData.disappearingEntities[disappearEntityIndex.value];

        var _randomTrue = GiveRandomIndex();

        cards[_randomTrue].SetData(true, _disapperingEntityData.name, _disapperingEntityData.sprite);

        int _random = -1;
        int _max = _disapperingEntityData.wrongSpriteAlbum.spriteList.Count;

        for (int i = 0; i < cards.Length - 1; i++)
        {
            var _randomWrong = GiveRandomIndex();

            _random = GiveUniqueRandom(_random, _max);
            cards[_randomWrong].SetData(false, string.Empty, _disapperingEntityData.wrongSpriteAlbum.spriteList[_random]);
        }
    }
    int GiveUniqueRandom(int random, int max)
    {
        var _random = Random.Range(0, max);

        if (random == _random)
            return GiveUniqueRandom(random, max);

        return _random;
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