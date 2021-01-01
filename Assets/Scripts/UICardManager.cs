using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class UICardManager : MonoBehaviour
{
    public UICard[] cards;
    public CurrentLevelData currentLevelData;
    public SharedInt disappearEntityIndex;
    public EventListenerDelegateResponse levelLoadedResponse;

    [HideInInspector]
    public List<int> randomOrder;

    private void OnEnable()
    {
        levelLoadedResponse.OnEnable();
    }

    private void OnDisable()
    {
        levelLoadedResponse.OnDisable();
    }
    private void Start()
    {
        levelLoadedResponse.response = SetCardsData;
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