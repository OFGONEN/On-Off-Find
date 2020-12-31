using System.Collections;
using System.Collections.Generic;
using FFStudio;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "FF/Data/LevelData")]
public class CustomLevelData : LevelData
{
    public int sceneBuildIndex;
    public Vector3 cameraStartPosition;
    public Vector3 cameraEndPosition;
    public Vector3 cameraStartRotation;
    public Vector3 cameraEndRotation;
    public Color ambientLightDefaultColor;
    public string[] disappearingEntityNames;
}
