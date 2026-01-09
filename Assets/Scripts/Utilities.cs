using System;
using System.IO;
using UnityEngine;

public static class Utilities
{
    public static void TryDeleteFile(string path)
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

    public static void TryDeleteDirectory(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"Failed trying to delete folder at {path}. Folder does not exist.");
        }
        try
        {
            Directory.Delete(path, true);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}