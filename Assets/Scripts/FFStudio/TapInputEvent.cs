using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    [CreateAssetMenu(fileName = "TapInputEvent", menuName = "FF/Event/TapInputEvent")]
    public class TapInputEvent : GameEvent
    {
        [HideInInspector]
        public int tapCount;

    }
}