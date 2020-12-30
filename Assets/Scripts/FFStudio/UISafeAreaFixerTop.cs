using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    public class UISafeAreaFixerTop : MonoBehaviour
    {
        public RectTransform uiRectTransform;
        private void Awake()
        {
            var _postion = uiRectTransform.anchoredPosition;
            _postion.y += Mathf.Sign(_postion.y) * (Screen.height - Screen.safeArea.height - Screen.safeArea.position.y);

            uiRectTransform.anchoredPosition = _postion;
        }
    }
}