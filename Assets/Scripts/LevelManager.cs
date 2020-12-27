using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CustomLevelData levelData;
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;
    public string reappearEntity;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position = levelData.cameraStartPosition;
        mainCamera.transform.rotation = Quaternion.Euler(levelData.cameraStartRotation);
    }

    void DisappearAllEntities()
    {
        foreach (var entityName in levelData.disappearingEntityNames)
        {
            DisappearingEntity _entity;
            disappearingEntitySet.itemDictionary.TryGetValue(entityName, out _entity);
            _entity.Disappear();
        }
    }

    void ReappearEntity()
    {
        DisappearingEntity _entity;
        disappearedEntitySet.itemDictionary.TryGetValue(reappearEntity, out _entity);
        _entity.Reappear();
    }
}
