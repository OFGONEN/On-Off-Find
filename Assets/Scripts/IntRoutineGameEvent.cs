using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntRoutineEvent", menuName = "FF/Event/IntRoutineEvent")]
public class IntRoutineGameEvent : RoutineGameEvent
{
    [HideInInspector]
    public int value;
    public CustomGameSettings gameSettings;
    WaitForSeconds waitForSeconds;

    public override void StartRoutine(MonoBehaviour routineOwner)
    {
        if (waitForSeconds == null) waitForSeconds = new WaitForSeconds(gameSettings.intRoutineWaitDuration);

        Raise();
        routine = routineOwner.StartCoroutine(EventRoutine());
    }
    public override void EndRoutine()
    {
        routineEndEvent.Raise();
        routine = null;
    }
    IEnumerator EventRoutine()
    {
        while (value > 0)
        {
            routineTickEvent.Raise();
            yield return waitForSeconds;
            value--;
        }

        EndRoutine();
    }
}
