using System.Diagnostics;

public static class FFmpegEncoder
{
    public static void EncodeVideo(
        string ffmpegPath,
        string frameDir,
        string audioPath,
        string outputPath
    )
    {
        string videoTemp = $"{frameDir}/video.mp4";

        Run($"{ffmpegPath}",
            $"-y -framerate 60 -i {frameDir}/frame_%05d.png " +
            "-c:v libx264 -pix_fmt yuv420p video.mp4");

        Run($"{ffmpegPath}",
            $"-y -i video.mp4 -i {audioPath} " +
            "-c:v copy -c:a aac {outputPath}");
    }

    static void Run(string exe, string args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = exe,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }
}
