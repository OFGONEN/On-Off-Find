using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIEntity : MonoBehaviour
{
    public CustomGameSettings gameSettings;
    public RectTransform uiTransform;
    public RectTransform destinationTransform;
    public Vector3 startPosition;
    public virtual void Start()
    {
        startPosition = uiTransform.position;
    }
    public virtual void GoTargetPosition()
    {
        uiTransform.DOMove(destinationTransform.position, gameSettings.uiEntityTweenDuration);
    }
    public virtual void GoStartPosition()
    {
        uiTransform.DOMove(startPosition, gameSettings.uiEntityTweenDuration);
    }
}
