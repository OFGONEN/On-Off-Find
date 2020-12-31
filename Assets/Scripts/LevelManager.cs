using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using FFStudio;

public class LevelManager : MonoBehaviour
{
    public Light globalLight;
    public CurrentLevelData currentLevelData;
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;

    public EventListenerDelegateResponse startLevelListener;
    public EventListenerDelegateResponse cleanUpLevelListener;
    public EventListenerDelegateResponse reappearEntityListener;
    Camera mainCamera;

    private void OnEnable()
    {
        startLevelListener.OnEnable();
        reappearEntityListener.OnEnable();
        cleanUpLevelListener.OnEnable();
    }
    private void OnDisable()
    {
        startLevelListener.OnDisable();
        reappearEntityListener.OnDisable();
        cleanUpLevelListener.OnDisable();
    }

    private void Start()
    {
        startLevelListener.response = SetUpLevel;
        cleanUpLevelListener.response = CleanUpLevel;

        reappearEntityListener.response = () =>
            ReappearEntity((reappearEntityListener.gameEvent as StringGameEvent).value);
    }

    void SetUpLevel()
    {
        var _levelData = currentLevelData.levelData;
        var _settings = currentLevelData.gameSettings;

        mainCamera = Camera.main;
        mainCamera.transform.position = _levelData.cameraStartPosition;
        mainCamera.transform.rotation = Quaternion.Euler(_levelData.cameraStartRotation);

        foreach (var pair in disappearingEntitySet.itemDictionary)
        {
            var disappearingEntity = pair.Value;

            disappearingEntity.rayFX = ParticleSystemManager.instance.GiveRayFx();
            disappearingEntity.rightPlaceFX = ParticleSystemManager.instance.GiveRightPlaceFX();

            disappearingEntity.SetFX();
        }

        TurnOnLights(MoveCamera);
    }
    void CleanUpLevel()
    {
        TurnOffLights();
    }
    void MoveCamera()
    {
        var _levelData = currentLevelData.levelData;
        var _settings = currentLevelData.gameSettings;

        mainCamera.transform.DOMove(_levelData.cameraEndPosition, _settings.cameraTweenDuration);
        mainCamera.transform.DORotate(_levelData.cameraEndRotation, _settings.cameraTweenDuration);
    }

    void TurnOnLights(TweenCallback onCompleteDelegate)
    {
        var _duration = currentLevelData.gameSettings.lightTurnOnDuration;

        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1, _duration).OnComplete(onCompleteDelegate);
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, currentLevelData.levelData.ambientLightDefaultColor, _duration);
    }
    void TurnOffLights()
    {
        var _duration = currentLevelData.gameSettings.lightTurnOffDuration;

        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0, _duration);
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, Color.black, _duration);
    }
    [Button]
    void DisappearAllEntities()
    {
        foreach (var entity in currentLevelData.levelData.disappearingEntities)
        {
            DisappearingEntity _entity;
            disappearingEntitySet.itemDictionary.TryGetValue(entity.name, out _entity);
            _entity.Disappear();
        }
    }

    void ReappearEntity(string reappearEntity)
    {
        DisappearingEntity _entity;
        disappearedEntitySet.itemDictionary.TryGetValue(reappearEntity, out _entity);
        _entity.Reappear();
    }
}
