using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace FFStudio
{
    public class UIEntity : MonoBehaviour
    {
        public CustomGameSettings gameSettings;
        public RectTransform uiTransform;
        public RectTransform destinationTransform;
        [HideInInspector]
        public Vector3 startPosition;
        public virtual void Start()
        {
            startPosition = uiTransform.position;
        }
        public virtual Tween GoTargetPosition()
        {
            return uiTransform.DOMove(destinationTransform.position, gameSettings.uiEntityTweenDuration);
        }
        public virtual Tween GoStartPosition()
        {
            return uiTransform.DOMove(startPosition, gameSettings.uiEntityTweenDuration);
        }
    }
}