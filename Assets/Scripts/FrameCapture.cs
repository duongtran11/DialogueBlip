using System.Collections;
using System.IO;
using UnityEngine;

public class FrameCapture : MonoBehaviour
{
    public RenderTexture rt;
    public int CapturedFrameCount => _frameIndex;

    bool _recording;
    int _frameIndex;

    public void StartCapture()
    {
        _frameIndex = 0;
        _recording = true;
        StartCoroutine(CaptureLoop());
    }

    public void StopCapture()
    {
        _recording = false;
    }

    IEnumerator CaptureLoop()
    {
        Texture2D tex = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        while (_recording)
        {
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
            tex.Apply();

            File.WriteAllBytes(
                $"{PathUtil.FrameDir}/frame_{_frameIndex:D05}.png",
                tex.EncodeToPNG()
            );

            _frameIndex++;
            yield return new WaitForEndOfFrame();
        }

        RenderTexture.active = null;
        Destroy(tex);
    }
}

