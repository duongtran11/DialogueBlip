using System.Diagnostics;
using System.IO;

public static class FFmpegEncoder
{
    public static void EncodeVideo(float fps)
    {
        string framePattern = Path.Combine(
            PathUtil.ExportDir,
            "frame_%05d.png"
        ).Replace("\\", "/");

        string outputVideo = Path.Combine(
            PathUtil.ExportDir,
            "video.mp4"
        ).Replace("\\", "/");

        string args =
            $"-y -framerate \"{fps}\" -i \"{framePattern}\" " +
            "-c:v libx264 -pix_fmt yuv420p " +
            $"\"{outputVideo}\"";

        Process encode = RunFFmpeg(
            PathUtil.FFmpegPath,
            args,
            PathUtil.ExportDir
        );
        encode.WaitForExit();

        string finalVideo = Path.Combine(
            PathUtil.ExportDir,
            "final.mp4"
        ).Replace("\\", "/");

        var audioPath = PathUtil.AudioPath.Replace("\\", "/");

        string mergeArgs =
            $"-y -i \"{outputVideo}\" -i \"{audioPath}\" " +
            "-c:v copy -c:a aac " +
            $"\"{finalVideo}\"";

        RunFFmpeg(
            PathUtil.FFmpegPath,
            mergeArgs,
            PathUtil.ExportDir
        );
    }

    static Process RunFFmpeg(string exe, string args, string workingDir)
    {
        var psi = new ProcessStartInfo
        {
            FileName = exe,
            Arguments = args,
            WorkingDirectory = workingDir,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var process = Process.Start(psi);
        return process;
    }
}
