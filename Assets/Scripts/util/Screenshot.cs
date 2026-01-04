using UnityEngine;

public class Screenshot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            string path = Application.dataPath + "/screenshot.png";
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log("Screenshot gespeichert: " + path);
        }
    }
}
