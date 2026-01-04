using UnityEngine;

public class RenderSetup : MonoBehaviour
{
    public Camera renderCamera;
    public RenderTexture renderTexture;

    public void Setup()
    {
        renderTexture = new RenderTexture(1920, 1080, 24);
        renderTexture.antiAliasing = 1;
        renderCamera.targetTexture = renderTexture;
    }
}
