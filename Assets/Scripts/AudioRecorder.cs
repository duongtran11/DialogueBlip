using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    WavWriter writer;

    public void StartRecord(string path)
    {
        writer = new WavWriter(path);
    }

    public void StopRecord()
    {
        writer?.Close();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        writer?.Write(data, channels);
    }
}
