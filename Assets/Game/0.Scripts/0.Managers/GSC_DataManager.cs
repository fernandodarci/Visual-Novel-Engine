using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GSC_DataManager : GSC_Singleton<GSC_DataManager>
{
    #region Fields

    private GSC_ContainerUnit SystemData;
    private List<string> SaveList;
    private GSC_ContainerUnit CurrentSaveData;

    #endregion

    #region Initialization

    private string GetSaveFileName()
    {
        DateTime now = DateTime.Now;
        return $"SaveFile {now.Day}-{now.Month}-{now.Year} {now.Hour}h{now.Minute}m{now.Second}";
    }

    public void InitializeData()
    {
        // Cria a pasta de salvamento, se não existir
        if (!Directory.Exists(GSC_Constants.GameSavePath))
        {
            Directory.CreateDirectory(GSC_Constants.GameSavePath);
        }

        // Processa os dados do sistema
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

        // Processa os arquivos de salvamento
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
                // Arquivo inválido, ignora
            }
        }

        // Cria um novo container para o salvamento atual
        CurrentSaveData = new GSC_ContainerUnit(GetSaveFileName());
    }

    #endregion

    #region Save / Load Methods

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
            // Possível log de erro
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
                    // Possível log de erro
                }
            }
        }
    }

    #endregion

    #region Setters

    public void AddOrChangeValue(string name, bool value, bool system = false)
    {
        if (system)
        {
            SystemData.Set(name, value);
            SaveData(true);
        }
        else
        {
            CurrentSaveData.Set(name, value);
        }
    }

    public void AddOrChangeValue(string name, int value, bool system = false)
    {
        if (system)
        {
            SystemData.Set(name, value);
            SaveData(true);
        }
        else
        {
            CurrentSaveData.Set(name, value);
        }
    }

    public void AddOrChangeValue(string name, float value, bool system = false)
    {
        if (system)
        {
            SystemData.Set(name, value);
            SaveData(true);
        }
        else
        {
            CurrentSaveData.Set(name, value);
        }
    }

    public void AddOrChangeValue(string name, string value, bool system = false)
    {
        if (system)
        {
            SystemData.Set(name, value);
            SaveData(true);
        }
        else
        {
            CurrentSaveData.Set(name, value);
        }
    }
    #endregion

    #region Getters
    public bool GetBooleanValue(string keyToCompare, bool system, out bool value)
    {
        value = false;
        if (system && SystemData.HasBoolean(keyToCompare))
        {
            value = SystemData.GetBoolean(keyToCompare);
            return true;
        }
        else if (CurrentSaveData.HasBoolean(keyToCompare))
        {
            value = CurrentSaveData.GetBoolean(keyToCompare);
            return true;
        }
        return false;
    }

    public bool GetIntegerValue(string keyToCompare, bool system, out int value)
    {
        value = int.MinValue;
        if (system && SystemData.HasInteger(keyToCompare))
        {
            value = SystemData.GetInteger(keyToCompare);
            return true;
        }
        else if (CurrentSaveData.HasInteger(keyToCompare))
        {
            value = CurrentSaveData.GetInteger(keyToCompare);
            return true;
        }
        return false;
    }

    public bool GetFloatValue(string keyToCompare, bool system, out float value)
    {
        value = float.MinValue;
        if (system && SystemData.HasFloat(keyToCompare))
        {
            value = SystemData.GetFloat(keyToCompare);
            return true;
        }
        else if (CurrentSaveData.HasInteger(keyToCompare))
        {
            value = CurrentSaveData.GetInteger(keyToCompare);
            return true;
        }
        return false;
    }

    public bool GetStringValue(string keyToCompare, bool system, out string value)
    {
        value = string.Empty;
        if (system && SystemData.HasString(keyToCompare))
        {
            value = SystemData.GetString(keyToCompare);
            return true;
        }
        else if (CurrentSaveData.HasString(keyToCompare))
        {
            value = CurrentSaveData.GetString(keyToCompare);
            return true;
        }
        return false;
    }

    #endregion

    #region Comparison Methods

    public bool CompareBoolean(bool system, string keyToCompare, bool isVal, bool value)
    {
        if (system)
        {
            return isVal ? SystemData.CompareBool(keyToCompare, value)
                         : !SystemData.CompareBool(keyToCompare, value);
        }
        else
        {
            return isVal ? CurrentSaveData.CompareBool(keyToCompare, value)
                         : !CurrentSaveData.CompareBool(keyToCompare, value);
        }
    }

    public bool CompareInteger(bool system, string keyToCompare, GSC_NumericComparator comp, int value)
    {
        if (system)
            return SystemData.CompareInteger(keyToCompare, comp, value);
        else
            return !CurrentSaveData.CompareInteger(keyToCompare, comp, value);
    }

    public bool CompareFloat(bool system, string keyToCompare, GSC_NumericComparator comp, float value)
    {
        if (system)
            return SystemData.CompareFloat(keyToCompare, comp, value);
        else
            return !CurrentSaveData.CompareFloat(keyToCompare, comp, value);
    }

    public bool CompareString(bool system, string keyToCompare, bool isVal, string value)
    {
        if (system)
        {
            return isVal ? SystemData.CompareString(keyToCompare, value)
                         : !SystemData.CompareString(keyToCompare, value);
        }
        else
        {
            return isVal ? CurrentSaveData.CompareString(keyToCompare, value)
                         : !CurrentSaveData.CompareString(keyToCompare, value);
        }
    }

    #endregion

    #region Operate Commands

    public void Operate(bool system, string keyToOperate, bool set, bool value)
    {
        if (set)
            AddOrChangeValue(keyToOperate, value, system);
        else
            RemoveBoolean(keyToOperate, system);
    }

    public void Operate(bool system, string keyToOperate, GSC_NumericOperator op, int value)
    {
        bool has = GetIntegerValue(keyToOperate, system, out int val);
        switch (op)
        {
            case GSC_NumericOperator.SET:
                AddOrChangeValue(keyToOperate, val, system);
                break;
            case GSC_NumericOperator.ADD:
                AddOrChangeValue(keyToOperate, has ? val + value : value, system);
                break;
            case GSC_NumericOperator.SUBTRACT:
                AddOrChangeValue(keyToOperate, has ? val - value : -value, system);
                break;
            case GSC_NumericOperator.UNSET:
                if (has)
                    RemoveInteger(keyToOperate, system);
                break;
        }
    }

    public void Operate(bool system, string keyToOperate, GSC_NumericOperator op, float value)
    {
        bool has = GetFloatValue(keyToOperate, system, out float val);
        switch (op)
        {
            case GSC_NumericOperator.SET:
                AddOrChangeValue(keyToOperate, val, system);
                break;
            case GSC_NumericOperator.ADD:
                AddOrChangeValue(keyToOperate, has ? val + value : value, system);
                break;
            case GSC_NumericOperator.SUBTRACT:
                AddOrChangeValue(keyToOperate, has ? val - value : -value, system);
                break;
            case GSC_NumericOperator.UNSET:
                if (has)
                    RemoveFloat(keyToOperate, system);
                break;
        }
    }

    public void Operate(bool system, string keyToOperate, bool set, string value)
    {
        if (set)
            AddOrChangeValue(keyToOperate, value, system);
        else
            RemoveString(keyToOperate, system);
    }
    public void RemoveCondition(GSC_Conditions condition)
    {
        if (condition != null)
        {
            if (condition is GSC_BooleanCondition @b) RemoveBoolean(b.KeyToCompare, b.System);
            if (condition is GSC_IntegerCondition @i) RemoveInteger(i.KeyToCompare, i.System);
            if (condition is GSC_FloatCondition @f) RemoveFloat(f.KeyToCompare, f.System);
            if (condition is GSC_StringCondition @s) RemoveString(s.KeyToCompare, s.System);
        }
    }
    #endregion

    #region Private Removal Methods

    private void RemoveInteger(string keyToOperate, bool system)
    {
        if (system)
            SystemData.RemoveInteger(keyToOperate);
        else
            CurrentSaveData.RemoveInteger(keyToOperate);
    }

    private void RemoveFloat(string keyToOperate, bool system)
    {
        if (system)
            SystemData.RemoveFloat(keyToOperate);
        else
            CurrentSaveData.RemoveFloat(keyToOperate);
    }

    private void RemoveString(string keyToOperate, bool system)
    {
        if (system)
            SystemData.RemoveString(keyToOperate);
        else
            CurrentSaveData.RemoveString(keyToOperate);
    }

    private void RemoveBoolean(string keyToOperate, bool system)
    {
        if (system)
            SystemData.RemoveBoolean(keyToOperate);
        else
            CurrentSaveData.RemoveBoolean(keyToOperate);
    }

    #endregion

    #region Miscellaneous

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

  
    #endregion

}
