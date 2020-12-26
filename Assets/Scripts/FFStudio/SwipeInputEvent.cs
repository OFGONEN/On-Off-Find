using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    [CreateAssetMenu(fileName = "SwipeInputEvent", menuName = "FF/Event/SwipeInputEvent")]
    public class SwipeInputEvent : GameEvent
    {
        [HideInInspector]
        public Vector2 swipeDirection = Vector2.zero;

        public void ReceiveInput(Vector2 swipeDelta)
        {
            swipeDirection = DecideDirection(Vector2.Angle(Vector2.right, swipeDelta), swipeDelta);

            if (swipeDirection != Vector2.zero)
                Raise();
        }
        Vector2 DecideDirection(float unsignedAngle, Vector2 delta)
        {
            if (unsignedAngle >= 140)
            {
                return Vector2.left;
            }
            else if (50 <= unsignedAngle && unsignedAngle <= 130)
            {
                if (delta.y >= 0)
                {
                    return Vector2.up;
                }
                else
                {
                    return Vector2.down;
                }
            }
            else if (unsignedAngle <= 40)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.zero;
            }
        }
    }
}