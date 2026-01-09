using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

public static class FFmpegEncoder
{
    public static void EncodeVideo(float fps)
    {
        string framePattern = Path.Combine(
            PathUtil.FrameDir,
            "frame_%05d.png"
        ).Replace("\\", "/");

        string outputVideo = PathUtil.OutputVideoPath;
        string args =
            $"-y -framerate \"{fps}\" -i \"{framePattern}\" " +
            "-c:v prores_ks -pix_fmt yuva444p10le -b:v 0 -crf 30 -auto-alt-ref 0 " +
            $"\"{outputVideo}\"";

        Process encode = RunFFmpeg(
            PathUtil.FFmpegPath,
            args,
            PathUtil.ExportDir
        );
        encode.WaitForExit();

        string finalVideo = PathUtil.FinalVideoPath;
        var audioPath = PathUtil.AudioPath;

        string mergeArgs =
            $"-y -i \"{outputVideo}\" -i \"{audioPath}\" " +
            "-c:v copy -c:a aac " +
            $"\"{finalVideo}\"";

        var merge = RunFFmpeg(
            PathUtil.FFmpegPath,
            mergeArgs,
            PathUtil.ExportDir
        );
        merge.WaitForExit();
        Cleanup();
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

    static void Cleanup()
    {
        Utilities.TryDeleteFile(PathUtil.AudioPath);
        Utilities.TryDeleteFile(PathUtil.OutputVideoPath);
        Utilities.TryDeleteDirectory(PathUtil.FrameDir);
        Debug.Log("Cleanup finished");
    }
}
