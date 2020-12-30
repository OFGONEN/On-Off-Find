using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using FFStudio;

public class LevelManager : MonoBehaviour
{
    public CurrentLevelData currentLevelData;
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;
    Camera mainCamera;

    private void Start()
    {
        var _levelData = currentLevelData.levelData;
        var _settings = currentLevelData.gameSettings;

        mainCamera = Camera.main;
        mainCamera.transform.position = _levelData.cameraStartPosition;
        mainCamera.transform.rotation = Quaternion.Euler(_levelData.cameraStartRotation);

        mainCamera.transform.DOMove(_levelData.cameraEndPosition, _settings.cameraTweenDuration);
        mainCamera.transform.DORotate(_levelData.cameraEndRotation, _settings.cameraTweenDuration);

        SetUpLevel();
    }

    void SetUpLevel()
    {
        foreach (var pair in disappearingEntitySet.itemDictionary)
        {
            var disappearingEntity = pair.Value;

            disappearingEntity.rayFX = ParticleSystemManager.instance.GiveRayFx();
            disappearingEntity.rightPlaceFX = ParticleSystemManager.instance.GiveRightPlaceFX();

            disappearingEntity.SetFX();
        }
    }

    void DisappearAllEntities()
    {
        foreach (var entityName in currentLevelData.levelData.disappearingEntityNames)
        {
            DisappearingEntity _entity;
            disappearingEntitySet.itemDictionary.TryGetValue(entityName, out _entity);
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
