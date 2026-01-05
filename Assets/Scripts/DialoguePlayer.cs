using System.Collections;
using TMPro;
using UnityEngine;

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

    float _speed;
    float _fast = 0.04f;
    float _slow = 0.09f;

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

    public IEnumerator Play(
        string speaker,
        string text,
        float interval,
        bool isMale
    )
    {
        nameText.text = speaker;
        dialogueText.text = "";

        _speed = DialogueSpeedCalculator.CalculateSpeed(text, interval);
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
                    _speed = _slow;
                    break;

                case DialogueFormat.Fast:
                    _speed = _fast;
                    break;

                default:
                    dialogueText.text += c;
                    Blip.TryPlay(c);
                    yield return new WaitForSeconds(_speed);
                    break;
            }
        }
    }

    public IEnumerator Generate(
        string name,
        string text,
        float interval,
        bool male
    )
    {
        _audioRecorder.StartRecord(PathUtil.AudioPath);

        StartCoroutine(_frameCapture.Capture(interval + 0.3f));
        yield return Play(name, text, interval, male);

        _audioRecorder.StopRecord();

        FFmpegEncoder.EncodeVideo(
            PathUtil.FFmpegPath,
            _frameCapture.outputDir,
            "dialogue.wav",
            "final.mp4"
        );
    }
}
