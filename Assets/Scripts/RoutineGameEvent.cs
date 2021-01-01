using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public abstract class RoutineGameEvent : GameEvent
{
    public GameEvent routineEndEvent;
    public GameEvent routineTickEvent;
    public Coroutine routine;

    public abstract void StartRoutine(MonoBehaviour routineOwner);
    public abstract void EndRoutine();
}
