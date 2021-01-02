using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[CreateAssetMenu(fileName = "SharedInt", menuName = "FF/Data/Shared/Int")]
public class SharedInt : ScriptableObject
{
    public int value;

    public EventListenerDelegateResponse cleanUpListener;

    private void OnEnable()
    {
        cleanUpListener.OnEnable();
        cleanUpListener.response = () => value = 0;
    }
    private void OnDisable()
    {
        cleanUpListener.OnDisable();
    }
}
