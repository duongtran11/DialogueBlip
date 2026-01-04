using System.Collections;
using UnityEngine;

public class VideoExportController : MonoBehaviour
{
    public RenderSetup render;
    public FrameCapture capture;
    public DialoguePlayer dialogue;
    public AudioRecorder audioRecorder;

    public IEnumerator Generate(
        string name,
        string text,
        float interval,
        bool male
    )
    {
        render.Setup();
        audioRecorder.StartRecord("dialogue.wav");

        StartCoroutine(capture.Capture(interval + 0.3f));
        yield return dialogue.Play(name, text, interval, male);

        audioRecorder.StopRecord();

        FFmpegEncoder.EncodeVideo(
            PathUtil.FFmpegPath,
            capture.outputDir,
            "dialogue.wav",
            "final.mp4"
        );
    }
}
