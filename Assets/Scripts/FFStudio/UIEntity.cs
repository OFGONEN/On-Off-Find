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
        [Button]
        public virtual void GoTargetPosition()
        {
            uiTransform.DOMove(destinationTransform.position, gameSettings.uiEntityTweenDuration);
        }
        [Button]
        public virtual void GoStartPosition()
        {
            uiTransform.DOMove(startPosition, gameSettings.uiEntityTweenDuration);
        }
    }
}