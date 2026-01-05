using System.IO;
using UnityEngine;

public static class PathUtil
{
    public static string AppRoot
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + "/../";
#else
            return Application.dataPath + "/../";
#endif
        }
    }

    /// Folder chứa ffmpeg
    public static string FFmpegPath
    {
        get
        {
            return Path.Combine(Application.dataPath, "StreamingAssets", "ffmpeg-8.0.1-essentials_build/bin/ffmpeg.exe");
        }
    }

    /// Folder export video
    public static string ExportDir
    {
        get
        {
            string dir = Path.Combine(AppRoot, "Exports");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
    }

    /// Folder chứa frame PNG
    public static string FrameDir
    {
        get
        {
            string dir = Path.Combine(ExportDir, "Frames");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
    }

    /// Path file audio wav
    public static string AudioPath
    {
        get
        {
            return Path.Combine(ExportDir, "dialogue.wav");
        }
    }

    /// Path video output
    public static string VideoPath
    {
        get
        {
            return Path.Combine(ExportDir, "final.mp4");
        }
    }
}
