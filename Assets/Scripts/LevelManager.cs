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

    #region Events
    public GameEvent lightsTurnedOn;
    #endregion
    #region EventListeners
    public EventListenerDelegateResponse newLevelLoadingListener;
    public EventListenerDelegateResponse levelLoadedListener;
    public EventListenerDelegateResponse startLevelListener;
    public EventListenerDelegateResponse startLevelInSameRoomListener;
    public EventListenerDelegateResponse reappearEntityListener;
    public EventListenerDelegateResponse countDownEndListener;
    #endregion
    Camera mainCamera;
    WaitForSeconds waitForLightTurnOff;

    private void OnEnable()
    {
        levelLoadedListener.OnEnable();
        newLevelLoadingListener.OnEnable();
        startLevelListener.OnEnable();
        startLevelInSameRoomListener.OnEnable();
        reappearEntityListener.OnEnable();
        countDownEndListener.OnEnable();
    }
    private void OnDisable()
    {
        levelLoadedListener.OnDisable();
        newLevelLoadingListener.OnDisable();
        startLevelListener.OnDisable();
        startLevelInSameRoomListener.OnDisable();
        reappearEntityListener.OnDisable();
        countDownEndListener.OnDisable();
    }

    private void Start()
    {
        levelLoadedListener.response = SetUpLevel;
        newLevelLoadingListener.response = () => TurnOffLights(EmptyMethod);
        startLevelListener.response = () => MoveCameraEndPosition(EmptyMethod);

        startLevelInSameRoomListener.response = () =>
        {
            levelLoadedListener.response = EmptyMethod;
            MoveCameraStartPosition(() =>
            {
                SetUpLevel();
                startLevelListener.gameEvent.Raise();
                levelLoadedListener.response = SetUpLevel;
            });
        };

        countDownEndListener.response = CountDownEndResponse;

        waitForLightTurnOff = new WaitForSeconds(currentLevelData.gameSettings.lightTurnOffWaitDuration);

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

        TurnOnLights(EmptyMethod);
    }

    void MoveCameraStartPosition(TweenCallback onComplete)
    {
        var _levelData = currentLevelData.levelData;
        var _settings = currentLevelData.gameSettings;

        mainCamera.transform.DOMove(_levelData.cameraStartPosition, _settings.cameraTweenDuration).OnComplete(onComplete);
        mainCamera.transform.DORotate(_levelData.cameraStartRotation, _settings.cameraTweenDuration);
    }
    void MoveCameraEndPosition(TweenCallback onComplete)
    {
        var _levelData = currentLevelData.levelData;
        var _settings = currentLevelData.gameSettings;

        mainCamera.transform.DOMove(_levelData.cameraEndPosition, _settings.cameraTweenDuration).OnComplete(onComplete);
        mainCamera.transform.DORotate(_levelData.cameraEndRotation, _settings.cameraTweenDuration);
    }
    void CountDownEndResponse()
    {
        TurnOffLights(() =>
        DisappearAllEntities(() =>
        StartCoroutine(WaitForSecond(() =>
        TurnOnLights(() => lightsTurnedOn.Raise()), waitForLightTurnOff))));

    }
    void TurnOnLights(TweenCallback onComplete)
    {
        var _duration = currentLevelData.gameSettings.lightTurnOnDuration;

        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1, _duration).OnComplete(onComplete);
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, currentLevelData.levelData.ambientLightDefaultColor, _duration);
    }
    void TurnOffLights(TweenCallback onComplete)
    {
        var _duration = currentLevelData.gameSettings.lightTurnOffDuration;

        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0, _duration).OnComplete(onComplete);
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, Color.black, _duration);
    }
    void DisappearAllEntities(TweenCallback onComplete)
    {
        foreach (var entity in currentLevelData.levelData.disappearingEntities)
        {
            DisappearingEntity _entity;
            disappearingEntitySet.itemDictionary.TryGetValue(entity.name, out _entity);
            _entity.Disappear();
        }

        onComplete();
    }
    void ReappearEntity(string reappearEntity)
    {
        DisappearingEntity _entity;
        disappearedEntitySet.itemDictionary.TryGetValue(reappearEntity, out _entity);
        _entity.Reappear();
    }

    IEnumerator WaitForSecond(TweenCallback onComplete, WaitForSeconds wait)
    {
        yield return wait;
        onComplete();
    }

    void EmptyMethod()
    {

    }
}
