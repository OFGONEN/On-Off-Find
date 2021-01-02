using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using FFStudio;

public class CameraDrag : MonoBehaviour
{
    #region EventListener
    public EventListenerDelegateResponse cleanUpResponse;
    public EventListenerDelegateResponse cameraMovedEndResponse;
    #endregion
    public CustomCurrentLevelData currentLevel;
    public LeanManualRotate cameraRotator;
    Vector3 endRotation;
    Vector2 dragDelta;
    float[] clampValues;

    public delegate void UpdateMethod();
    UpdateMethod cameraUpdate;
    UpdateMethod cameraRotate;

    private void OnEnable()
    {
        cleanUpResponse.OnEnable();
        cameraMovedEndResponse.OnEnable();
    }
    private void OnDisable()
    {
        cleanUpResponse.OnEnable();
        cameraMovedEndResponse.OnEnable();
    }

    private void Awake()
    {
        cameraRotator.Multiplier = currentLevel.gameSettings.cameraTurnMultiplier;
        cameraUpdate = EmptyMethod;
        cameraRotate = EmptyMethod;

        clampValues = new float[2];
    }
    private void Start()
    {
        cleanUpResponse.response = () =>
        {
            cameraUpdate = EmptyMethod;
            cameraRotate = EmptyMethod;
        };

        cameraMovedEndResponse.response = () =>
        {
            SetClamp();
            cameraRotate = RotateCamera;
        };
    }
    public void Rotate(Vector2 drag)
    {
        dragDelta = drag;
        cameraRotate();
    }
    public void SetClamp()
    {
        endRotation = transform.rotation.eulerAngles;

        clampValues[0] = endRotation.y - currentLevel.gameSettings.cameraTurnDegree;
        clampValues[1] = endRotation.y + currentLevel.gameSettings.cameraTurnDegree;

        cameraUpdate = ClampCamera;
    }
    public void Update()
    {
        cameraUpdate();
    }
    public void RotateCamera()
    {
        dragDelta.y = 0;
        cameraRotator.RotateAB(dragDelta);
    }
    public void ClampCamera()
    {
        var _y = Mathf.Clamp(transform.rotation.eulerAngles.y, clampValues[0], clampValues[1]);

        transform.rotation = Quaternion.Euler(endRotation.x, _y, endRotation.z);
    }


    public void EmptyMethod()
    {

    }
}
