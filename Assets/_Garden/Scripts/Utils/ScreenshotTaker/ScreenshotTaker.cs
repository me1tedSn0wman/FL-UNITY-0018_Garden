using System;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void TakeScreenshot() {
        string name = DateTime.Now.ToString();
        ScreenCapture.CaptureScreenshot("GardenScreen"+name);
    }
}
