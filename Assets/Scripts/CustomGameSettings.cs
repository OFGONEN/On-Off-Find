using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;


[CreateAssetMenu(fileName = "GameSettings", menuName = "FF/Data/GameSettings")]
public class CustomGameSettings : GameSettings
{
    public float cameraTweenDuration;
    public float uiEntityTweenDuration;
    public int intRoutineWaitDuration;
    public float lightTurnOnDuration;
    public float lightTurnOffDuration;
    public float lightTurnOffWaitDuration;
    public int fxPoolCount;
}
