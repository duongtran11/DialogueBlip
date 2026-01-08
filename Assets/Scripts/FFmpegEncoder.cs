using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class FFmpegEncoder
{
    public static void EncodeVideo(float fps)
    {
        string framePattern = Path.Combine(
            PathUtil.ExportDir,
            "frame_%05d.png"
        ).Replace("\\", "/");

        string outputVideo = PathUtil.OutputVideoPath;
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
        // Cleanup();
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
        TryDeleteFile(PathUtil.AudioPath);
        TryDeleteFile(PathUtil.OutputVideoPath);
        string[] frames = Directory.GetFiles(PathUtil.ExportDir, "frame_*.png");
        foreach (string frame in frames)
        {
            TryDeleteFile(frame);
        }
        Debug.Log("Cleanup finished");
    }

    static void TryDeleteFile(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Failed trying to delete file at {path}. File does not exist.");
        }
        try
        {
            File.Delete(path);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
