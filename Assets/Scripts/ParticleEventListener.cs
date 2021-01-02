using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class ParticleEventListener : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    public EventListenerDelegateResponse particleStartListener;
    public EventListenerDelegateResponse particleStopListener;

    private void OnEnable()
    {
        particleStartListener.OnEnable();
        particleStopListener.OnEnable();
    }
    private void OnDisable()
    {
        particleStartListener.OnDisable();
        particleStopListener.OnDisable();
    }

    private void Start()
    {
        particleStartListener.response = StartParticles;
        particleStopListener.response = StopParticles;
    }

    void StartParticles()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }
    void StopParticles()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Clear();
            particleSystems[i].Stop();
        }
    }
}
