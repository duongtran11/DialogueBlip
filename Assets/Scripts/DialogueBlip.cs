using UnityEngine;

public class DialogueBlip : MonoBehaviour
{
    public AudioClip blipMale;
    public AudioClip blipFemale;
    public AudioSource audioSource;

    bool isMale;
    int counter;

    public void Init(bool male)
    {
        isMale = male;
        counter = 0;
    }

    public void TryPlay(char c)
    {
        if (!char.IsLetter(c)) return;

        counter++;
        if (counter % 2 == 1)
        {
            audioSource.PlayOneShot(
                isMale ? blipMale : blipFemale
            );
        }
    }
}
