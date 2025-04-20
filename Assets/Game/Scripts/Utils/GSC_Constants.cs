using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GSC_Constants
{
    public static string ResourcesPath => $"{Application.dataPath}\\Resources";
    public static string GameSavePath => $"{Application.persistentDataPath}\\Saves";
    public static string GameScriptPath => $"{Application.persistentDataPath}\\Scripts";
    
    public static bool SaveFile(string content, string path)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(path, false);
            writer.Write(content);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving file: {e.Message}");
            return false;
        }
    }

    public static string LoadFile(string path)
    {
        try
        {
            using StreamReader reader = new StreamReader(path);
            return reader.ReadToEnd();
        }
        catch
        {
            Debug.LogError($"Error loading file: {path}");
            return string.Empty;
        }
    }

    public static string[] LoadScriptFile(string path, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();

        if (File.Exists(Path.Combine(GameScriptPath, path)))
        {
            try
            {
                using StreamReader reader = new StreamReader(Path.Combine(GameScriptPath, path));
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if (includeBlankLines || !line.IsNullOrEmpty()) lines.Add(line);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading script file: {e.Message}");
            }
        }

        return lines.ToArray();
    }

    public static string[] ReadTextAsset(string name, bool includeBlankLines = true)
    {
        var asset = Resources.Load<TextAsset>(name);
        if(asset == null)
        {
            Debug.LogError($"Text asset '{name}' not found.");
            return new string[0];
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    public static string[] ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using(StringReader reader = new StringReader(asset.text))
        {
            string line;
            while (reader.Peek() != -1)
            {
                line = reader.ReadLine().Trim();
                if (includeBlankLines || !string.IsNullOrEmpty(line)) lines.Add(line);
            }
        }
        return lines.ToArray();
    }

    public static IEnumerator WaitForComplete(Func<bool> complete)
    {
        while (!complete())
        {
            yield return null;
        }
    }

    public static IEnumerator WaitForSeconds(float seconds, Func<bool> pause, Func<bool> ends)
    {
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (ends()) yield break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
        }
    }
}
