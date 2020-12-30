using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager instance = null;
    public CustomGameSettings gameSettings;
    public GameObject rayFX;
    public GameObject rightPlaceFX;
    public Stack<ParticleObserver> rayFXPool;
    public Stack<ParticleObserver> rightPlaceFXPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        for (int i = 0; i < gameSettings.fxPoolCount; i++)
        {
            rayFXPool.Push(CreateRayFX());
            rightPlaceFXPool.Push(CreateRightPlaceFX());
        }
    }
    private ParticleObserver GiveRayFx()
    {
        var _rayFX = rayFXPool.Peek();

        if (_rayFX != null) return rayFXPool.Pop();

        return CreateRayFX();
    }
    private ParticleObserver GiveRightPlaceFX()
    {
        var _rightPlaceFX = rightPlaceFXPool.Peek();

        if (_rightPlaceFX != null) return rightPlaceFXPool.Pop();

        return CreateRightPlaceFX();
    }
    private ParticleObserver CreateRayFX()
    {
        var _rayFX = GameObject.Instantiate(rayFX);
        _rayFX.SetActive(false);

        var _particleObserver = _rayFX.GetComponent<ParticleObserver>();
        _particleObserver.returnPool = rayFXPool;

        return _particleObserver;
    }
    private ParticleObserver CreateRightPlaceFX()
    {
        var _rightPlaceFX = GameObject.Instantiate(rightPlaceFX);
        _rightPlaceFX.SetActive(false);

        var _particleObserver = _rightPlaceFX.GetComponent<ParticleObserver>();
        _particleObserver.returnPool = rightPlaceFXPool;

        return _particleObserver;
    }
}
