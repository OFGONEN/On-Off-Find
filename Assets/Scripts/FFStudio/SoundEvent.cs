using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFStudio
{
    [CreateAssetMenu(fileName = "SoundEvent", menuName = "FF/Event/SoundEvent")]
    public class SoundEvent : GameEvent
    {
        public AudioClip audioClip;
    }
}