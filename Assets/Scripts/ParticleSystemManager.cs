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

        rayFXPool = new Stack<ParticleObserver>(gameSettings.fxPoolCount);
        rightPlaceFXPool = new Stack<ParticleObserver>(gameSettings.fxPoolCount);

        for (int i = 0; i < gameSettings.fxPoolCount; i++)
        {
            CreateRayFX();
            CreateRightPlaceFX();
        }
    }
    public ParticleObserver GiveRayFx()
    {
        if (rayFXPool.Count == 0)
            CreateRayFX();

        return rayFXPool.Pop();
    }
    public ParticleObserver GiveRightPlaceFX()
    {
        if (rightPlaceFXPool.Count == 0)
            CreateRightPlaceFX();

        return rightPlaceFXPool.Pop();
    }
    private void CreateRayFX()
    {
        var _rayFX = GameObject.Instantiate(rayFX);

        var _particleObserver = _rayFX.GetComponent<ParticleObserver>();
        _particleObserver.returnPool = rayFXPool;
        _rayFX.SetActive(false);
    }
    private void CreateRightPlaceFX()
    {
        var _rightPlaceFX = GameObject.Instantiate(rightPlaceFX);

        var _particleObserver = _rightPlaceFX.GetComponent<ParticleObserver>();
        _particleObserver.returnPool = rightPlaceFXPool;
        _rightPlaceFX.SetActive(false);
    }
}
