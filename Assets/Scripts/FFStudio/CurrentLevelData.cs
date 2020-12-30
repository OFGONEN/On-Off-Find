using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    public abstract class CurrentLevelData : ScriptableObject
    {
        public int currentLevel;
        public CustomGameSettings gameSettings;
        public CustomLevelData levelData;
        public void LoadCurrentLevelData()
        {
            if (currentLevel > gameSettings.maxLevelCount)
            {
                currentLevel = Random.Range(1, gameSettings.maxLevelCount);
            }

            levelData = Resources.Load<CustomLevelData>("LevelData_" + currentLevel);
        }
    }
}