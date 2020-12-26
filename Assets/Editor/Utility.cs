using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FFEditor
{
    public static class Utility
    {
        [MenuItem("FFStudios/TakeScreenShot")]
        public static void TakeScreenShot()
        {
            ScreenCapture.CaptureScreenshot("ScreenShot.png");
        }

        [MenuItem("FFStudios/Delete PlayerPrefs")]
        static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        [MenuItem("FFStudios/Save All Assets")]
        static void SaveAllAssets()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
