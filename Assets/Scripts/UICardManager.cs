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
    int randomOrderCount;
    List<int> randomSpriteOrder;
    int randomSpriteOrderCount;

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
        randomOrderCount = cards.Length;
        randomSpriteOrderCount = currentLevelData.levelData.spriteAlbum.spriteList.Count;

        randomOrder = new List<int>(cards.Length);
        randomSpriteOrder = new List<int>(randomSpriteOrderCount);
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
        ResetRandom(randomOrder, randomOrderCount);
        ResetRandom(randomSpriteOrder, randomSpriteOrderCount);

        var _disapperingEntityData = currentLevelData.levelData.disappearingEntities[disappearEntityIndex.value];
        var _spriteAlbum = currentLevelData.levelData.spriteAlbum;

        var _randomTrue = GiveRandomIndex(randomOrder);
        var _correctIndex = _disapperingEntityData.spriteIndex;

        RemoveCorrectIndexes();
        cards[_randomTrue].SetData(true, _disapperingEntityData.name, _spriteAlbum.spriteList[_correctIndex]);

        int _random = -1;
        int _max = _spriteAlbum.spriteList.Count;

        for (int i = 0; i < cards.Length - 1; i++)
        {
            var _randomWrong = GiveRandomIndex(randomOrder);
            _random = GiveRandomIndex(randomSpriteOrder);

            cards[_randomWrong].SetData(false, string.Empty, _spriteAlbum.spriteList[_random]);
        }
    }
    void RemoveCorrectIndexes()
    {
        var _disapperingEntityDatas = currentLevelData.levelData.disappearingEntities;

        // for (int i = _disapperingEntityDatas.Length - 1; i >= 0; i--)
        // {
        //     if (_disapperingEntityDatas[i].spriteIndex == randomSpriteOrder[i])
        //         randomSpriteOrder.RemoveAt(i);
        // }

        for (int x = 0; x < _disapperingEntityDatas.Length; x++)
        {
            for (int y = 0; y < randomSpriteOrder.Count; y++)
            {
                if (_disapperingEntityDatas[x].spriteIndex == randomSpriteOrder[y])
                {
                    randomSpriteOrder.RemoveAt(y);
                    break;
                }
            }
        }
    }
    int GiveRandomIndex(List<int> randomOrderList)
    {
        var _randomIndex = Random.Range(0, randomOrderList.Count);
        var _randomValue = randomOrderList[_randomIndex];

        randomOrderList.RemoveAt(_randomIndex);
        return _randomValue;
    }
    void ResetRandom(List<int> randomOrderList, int count)
    {
        randomOrderList.Clear();

        for (int i = 0; i < count; i++)
        {
            randomOrderList.Add(i);
        }
    }
}