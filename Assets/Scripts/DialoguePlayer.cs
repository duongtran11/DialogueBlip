using System.Collections;
using TMPro;
using UnityEngine;
public class DialoguePlayer : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public DialogueBlip blip;

    float speed;
    float fast = 0.04f;
    float slow = 0.08f;

    public IEnumerator Play(
        string speaker,
        string text,
        float interval,
        bool isMale
    )
    {
        nameText.text = speaker;
        dialogueText.text = "";

        speed = DialogueSpeedCalculator.CalculateSpeed(text, interval);
        blip.Init(isMale);

        foreach (char c in text)
        {
            switch (c)
            {
                case DialogueFormat.LineBreak:
                    dialogueText.text += "\n";
                    break;

                case DialogueFormat.Pause:
                    yield return new WaitForSeconds(speed);
                    break;

                case DialogueFormat.Slow:
                    speed = slow;
                    break;

                case DialogueFormat.Fast:
                    speed = fast;
                    break;

                default:
                    dialogueText.text += c;
                    blip.TryPlay(c);
                    yield return new WaitForSeconds(speed);
                    break;
            }
        }
    }
}
