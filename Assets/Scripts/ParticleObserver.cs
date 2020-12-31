using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class ParticleObserver : MonoBehaviour
{
    public ParticleSystem particles;
    [HideInInspector]
    public ParticleSystemRenderer particleRenderer;
    public EventListenerDelegateResponse cleanUpListener;
    public Stack<ParticleObserver> returnPool;
    public delegate void ParticleSystemStopDelegate();
    public ParticleSystemStopDelegate particleStoped;

    private void Awake()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        particleStoped = EmptyMethod;
    }
    private void Start()
    {
        cleanUpListener.response = () => gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        cleanUpListener.OnEnable();
    }
    private void OnDisable()
    {
        cleanUpListener.OnDisable();

        if (particles.isPlaying)
            particles.Stop();

        particleStoped = EmptyMethod;
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
