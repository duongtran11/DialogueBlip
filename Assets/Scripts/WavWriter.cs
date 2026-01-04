using System;
using System.IO;
using System.Text;

public class WavWriter
{
    FileStream stream;
    int sampleRate;
    int channels;
    int totalSamples;

    public WavWriter(string path, int sampleRate = 44100, int channels = 2)
    {
        this.sampleRate = sampleRate;
        this.channels = channels;
        stream = new FileStream(path, FileMode.Create);

        // placeholder header
        WriteHeader(0);
    }

    public void Write(float[] data, int ch)
    {
        if (stream == null) return;

        byte[] buffer = new byte[data.Length * 2];
        int offset = 0;

        for (int i = 0; i < data.Length; i++)
        {
            short value = (short)(Math.Clamp(data[i], -1f, 1f) * short.MaxValue);
            buffer[offset++] = (byte)(value & 0xff);
            buffer[offset++] = (byte)((value >> 8) & 0xff);
        }

        stream.Write(buffer, 0, buffer.Length);
        totalSamples += data.Length;
    }

    public void Close()
    {
        if (stream == null) return;

        long fileSize = stream.Length;
        stream.Seek(0, SeekOrigin.Begin);

        WriteHeader(fileSize - 8);

        stream.Close();
        stream = null;
    }

    void WriteHeader(long dataSize)
    {
        BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8);

        bw.Write(Encoding.ASCII.GetBytes("RIFF"));
        bw.Write((int)(dataSize + 36));
        bw.Write(Encoding.ASCII.GetBytes("WAVE"));

        bw.Write(Encoding.ASCII.GetBytes("fmt "));
        bw.Write(16);
        bw.Write((short)1); // PCM
        bw.Write((short)channels);
        bw.Write(sampleRate);
        bw.Write(sampleRate * channels * 2);
        bw.Write((short)(channels * 2));
        bw.Write((short)16);

        bw.Write(Encoding.ASCII.GetBytes("data"));
        bw.Write((int)dataSize);
    }
}
