using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Screenshotter : MonoBehaviour
{
    public bool screenshot = false;

    // Update is called once per frame
    void Update()
    {
        if (screenshot)
        {
            ScreenCapture.CaptureScreenshot("BG.png");

            screenshot = false;
        }
    }
}
