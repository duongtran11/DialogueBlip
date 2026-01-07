using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(FrameCapture))]
public class DialoguePlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueBlip Blip;
    [SerializeField] private AudioRecorder _audioRecorder;

    [Space]
    [Header("UI")]
    [SerializeField] private TMP_InputField NameInput;
    [SerializeField] private TMP_InputField DialogueInput;
    [SerializeField] private TMP_InputField IntervalInput;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    private FrameCapture _frameCapture;
    private float _generateTimeInSeconds;

    float _speed;
    float _fast = 1.2f;
    float _slow = 0.7f;

    void Awake()
    {
        _frameCapture = GetComponent<FrameCapture>();
    }

    public void StartPlayback()
    {
        if (string.IsNullOrEmpty(NameInput.text))
        {
            Debug.LogWarning("<b>Name</b> is empty. Playback canceled.");
            return;
        }
        if (string.IsNullOrEmpty(DialogueInput.text))
        {
            Debug.LogWarning("<b>Dialogue</b> is empty. Playback canceled.");
            return;
        }
        if (string.IsNullOrEmpty(IntervalInput.text))
        {
            Debug.LogWarning("<b>Interval</b> is empty. Playback canceled.");
            return;
        }

        StartCoroutine(Play(NameInput.text, DialogueInput.text, float.Parse(IntervalInput.text), true));
    }

    public void StartGenerate()
    {
        if (string.IsNullOrEmpty(NameInput.text))
        {
            Debug.LogWarning("<b>Name</b> is empty. Playback canceled.");
            return;
        }
        if (string.IsNullOrEmpty(DialogueInput.text))
        {
            Debug.LogWarning("<b>Dialogue</b> is empty. Playback canceled.");
            return;
        }
        if (string.IsNullOrEmpty(IntervalInput.text))
        {
            Debug.LogWarning("<b>Interval</b> is empty. Playback canceled.");
            return;
        }

        StartCoroutine(Generate(NameInput.text, DialogueInput.text, float.Parse(IntervalInput.text), true));
    }

    public IEnumerator Play(string speaker, string text, float dialogueSpeed, bool isMale)
    {
        nameText.text = speaker;
        dialogueText.text = "";

        float baseSpeed = dialogueSpeed;
        _speed = baseSpeed;
        Blip.Init(isMale);

        foreach (char c in text)
        {
            switch (c)
            {
                case DialogueFormat.LineBreak:
                    dialogueText.text += "\n";
                    break;

                case DialogueFormat.Pause:
                    yield return new WaitForSeconds(_speed);
                    break;

                case DialogueFormat.Slow:
                    _speed = baseSpeed * _slow;
                    break;

                case DialogueFormat.Fast:
                    _speed = baseSpeed * _fast;
                    break;

                default:
                    dialogueText.text += c;
                    Blip.TryPlay(c);
                    yield return new WaitForSeconds(_speed);
                    break;
            }
        }
    }

    public IEnumerator Generate(string name, string text, float dialogueSpeed, bool male)
    {
        _audioRecorder.StartRecord(PathUtil.AudioPath);
        Stopwatch stopwatch = Stopwatch.StartNew();

        _frameCapture.StartCapture();
        nameText.text = name;
        dialogueText.text = "";

        float baseSpeed = dialogueSpeed;
        _speed = baseSpeed;
        Blip.Init(male);

        foreach (char c in text)
        {
            switch (c)
            {
                case DialogueFormat.LineBreak:
                    dialogueText.text += "\n";
                    break;

                case DialogueFormat.Pause:
                    yield return new WaitForSeconds(_speed);
                    break;

                case DialogueFormat.Slow:
                    _speed = baseSpeed * _slow;
                    break;

                case DialogueFormat.Fast:
                    _speed = baseSpeed * _fast;
                    break;

                default:
                    dialogueText.text += c;
                    Blip.TryPlay(c);
                    yield return new WaitForSeconds(_speed);
                    break;
            }
        }
        _frameCapture.StopCapture();

        _audioRecorder.StopRecord();
        stopwatch.Stop();
        _generateTimeInSeconds = stopwatch.ElapsedMilliseconds / 1000f;
        var calculatedFPS = _frameCapture.CapturedFrameCount / _generateTimeInSeconds;

        FFmpegEncoder.EncodeVideo(calculatedFPS);
        _frameCapture.Cleanup();
    }
}
