using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

namespace FFStudio
{
    public class MobileInput : MonoBehaviour
    {
        public SwipeInputEvent swipeInputEvent;
        public TapInputEvent tapInputEvent;
        public void Swiped(Vector2 delta)
        {
            swipeInputEvent.ReceiveInput(delta);
        }

        public void Tapped(int count)
        {
            tapInputEvent.tapCount = count;

            tapInputEvent.Raise();
        }
    }
}