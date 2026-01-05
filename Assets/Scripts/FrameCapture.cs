using System.Collections;
using System.IO;
using UnityEngine;

public class FrameCapture : MonoBehaviour
{
    public RenderTexture rt;
    public string outputDir;
    public int fps = 60;

    void OnValidate()
    {
        outputDir = string.IsNullOrEmpty(outputDir) ? Path.Combine(Application.dataPath, "Exports") : outputDir;
    }

    public IEnumerator Capture(float duration)
    {
        int total = Mathf.CeilToInt(duration * fps);

        for (int i = 0; i < total; i++)
        {
            yield return new WaitForEndOfFrame();

            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
            tex.Apply();

            File.WriteAllBytes(
                $"{outputDir}/frame_{i:D05}.png",
                tex.EncodeToPNG()
            );

            Object.Destroy(tex);
        }

        RenderTexture.active = null;
    }
}
