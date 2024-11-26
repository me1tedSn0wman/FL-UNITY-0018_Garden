using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ScreenshotTaker))]
public class ScreenshotTakerUI : Editor
{
    private ScreenshotTaker screenshotTaker;

    private void OnEnable()
    {
        screenshotTaker = (ScreenshotTaker)target;
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();
        if (GUILayout.Button("Take Screenshot"))
        {
            screenshotTaker.TakeScreenshot();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif