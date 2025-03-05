using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GSC_DataManager : GSC_Singleton<GSC_DataManager>
{
    private GSC_ContainerUnit SystemData;
    private List<string> SaveList;
    private GSC_ContainerUnit CurrentSaveData;

    private string GetSaveFileName()
    {
        DateTime now = DateTime.Now;
        return $"SaveFile {now.Day}-{now.Month}-{now.Year} {now.Hour}h{now.Minute}m{now.Second}";
    }

    public void InitializeData()
    {
        if (!Directory.Exists(GSC_Constants.GameSavePath))
        {
            Directory.CreateDirectory(GSC_Constants.GameSavePath);
        }

        string systemDataFile = Path.Combine(GSC_Constants.GameSavePath, "SystemData.json");

        if (File.Exists(systemDataFile))
        {
            try
            {
                string json = File.ReadAllText(systemDataFile);
                SystemData = GSC_ContainerUnit.FromJson(json);
            }
            catch (Exception)
            {
                SystemData = new GSC_ContainerUnit("SystemData");
                SystemData.Set("Guid", Guid.NewGuid().ToString());
                SaveData(true);
            }
        }
        else
        {
            SystemData = new GSC_ContainerUnit("SystemData");
            SystemData.Set("Guid", Guid.NewGuid().ToString());
            SaveData(true);
        }

        string[] saveFiles = Directory.GetFiles(GSC_Constants.GameSavePath, "SaveFile *.json");

        SaveList = new List<string>();

        foreach (string file in saveFiles)
        {
            try
            {
                string json = File.ReadAllText(file);
                GSC_ContainerUnit saveUnit = GSC_ContainerUnit.FromJson(json);
                if (saveUnit != null && saveUnit.HasString("Guid") &&
                    saveUnit.GetString("Guid") == SystemData.GetString("Guid"))
                {
                    SaveList.Add(saveUnit.Calling);
                }
            }
            catch
            {
                // Se um arquivo não puder ser carregado, ele é ignorado
            }
        }

        CurrentSaveData = new GSC_ContainerUnit(GetSaveFileName());
    }

    public void SaveData(bool system = false)
    {
        try
        {
            string file = system ? "SystemData.json" : $"{CurrentSaveData.Calling}.json";
            string contents = system ? SystemData.ToJson() : CurrentSaveData.ToJson();
            File.WriteAllText(Path.Combine(GSC_Constants.GameSavePath, file), contents);
        }
        catch (Exception ex)
        {
            // Log de erro pode ser adicionado aqui
        }
    }

    public string LoadData(string file)
    {
        try
        {
            string fileName = Path.GetFileName(file);
            string filePath = Path.Combine(GSC_Constants.GameSavePath, fileName);

            if (!File.Exists(filePath))
                return $"Error: Provided name {fileName} not found on system.";

            string json = File.ReadAllText(filePath);
            GSC_ContainerUnit saveUnit = GSC_ContainerUnit.FromJson(json);

            if (saveUnit != null && saveUnit.HasString("Guid") &&
                saveUnit.GetString("Guid") == SystemData.GetString("Guid"))
            {
                if (fileName == "SystemData.json")
                {
                    SystemData = saveUnit;
                    return "System Data loaded successfully";
                }
                else
                {
                    CurrentSaveData = saveUnit;
                    return $"Save File {CurrentSaveData.Calling} loaded successfully";
                }
            }
            return $"Error: Save file {fileName} does not match current system data.";
        }
        catch (Exception ex)
        {
            return $"Error: File can't be loaded. {ex.Message}";
        }
    }

    public void RemoveSave(string savefile)
    {
        if (SaveList.Contains(savefile))
        {
            SaveList.Remove(savefile);
            string filePath = Path.Combine(GSC_Constants.GameSavePath, $"{savefile}.json");
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    // Log pode ser adicionado aqui se necessário
                }
            }
        }
    }

    public void AddOrChangeSystemValue(string name, bool value)
    {
        SystemData.Set(name, value);
        SaveData(true);
    }

    public void AddOrChangeSaveValue(string name, bool value)
    {
        CurrentSaveData.Set(name, value);
    }

    public void AddOrChangeSystemValue(string name, int value)
    {
        SystemData.Set(name, value);
        SaveData(true);
    }

    public void AddOrChangeSaveValue(string name, int value)
    {
        CurrentSaveData.Set(name, value);
    }

    public void AddOrChangeSystemValue(string name, float value)
    {
        SystemData.Set(name, value);
        SaveData(true);
    }

    public void AddOrChangeSaveValue(string name, float value)
    {
        CurrentSaveData.Set(name, value);
    }

    public void AddOrChangeSystemValue(string name, string value)
    {
        SystemData.Set(name, value);
        SaveData(true);
    }

    public void AddOrChangeSaveValue(string name, string value)
    {
        CurrentSaveData.Set(name, value);
    }

    public string ProcessString(string input)
    {
        input = Regex.Replace(input, @"@system\((.*?)\)", match =>
            SystemData.GetAsString(match.Groups[1].Value));

        input = Regex.Replace(input, @"@save\((.*?)\)", match =>
            CurrentSaveData.GetAsString(match.Groups[1].Value));

        input = Regex.Replace(input, @"@info\((.*?)\)", match =>
            GSC_SystemInfo.GetAsString(match.Groups[1].Value));

        input = Regex.Replace(input, @"@name\((.*?)\)", match =>
        {
            string content = match.Groups[1].Value;
            if (content.Contains("=="))
            {
                string[] parts = content.Split(new string[] { "==" }, StringSplitOptions.None);
                return GSC_ProviderManager.Instance.GetName(parts[0].Trim(), parts[1].Trim());
            }
            return GSC_ProviderManager.Instance.GetName(content.Trim(), null);
        });

        return input;
    }

}
