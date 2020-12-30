using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObserver : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystemRenderer particleRenderer;
    public delegate void ParticleSystemStopDelegate();
    public ParticleSystemStopDelegate particleStoped;
    public Stack<ParticleObserver> returnPool;
    private void Awake()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        particleStoped = EmptyMethod;
    }
    private void OnDisable()
    {
        if (particles.isPlaying)
            particles.Stop();

        particleStoped = EmptyMethod;
        gameObject.SetActive(false);
        returnPool.Push(this);
    }

    private void OnParticleSystemStopped()
    {
        particleStoped();
    }
    void EmptyMethod()
    {

    }
}
