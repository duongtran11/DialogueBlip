using System.IO;
using UnityEngine;

public static class PathUtil
{
    /// Root folder của app (bên cạnh exe)
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
#if UNITY_STANDALONE_WIN
            return Path.Combine(AppRoot, "ffmpeg/ffmpeg.exe");
#elif UNITY_STANDALONE_OSX
            return Path.Combine(AppRoot, "ffmpeg/ffmpeg");
#else
            return "";
#endif
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
