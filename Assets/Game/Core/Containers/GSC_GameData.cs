using System;
using System.Collections.Generic;

[Serializable]
public class GSC_GameData
{
    public GSC_ParameterData MainData;
    public List<GSC_GameSave> GameSaves;
    public GSC_GameSave CurrentSave;

    public void Initialize()
    {
        MainData = new();
        GameSaves = new List<GSC_GameSave>();
        CurrentSave = new()
        {
            Label = string.Empty,
            Database = new(),
            StampDate = DateTime.Now,
        };
        GameSaves.Add(CurrentSave);
    }

    public void Set(GSC_Parameter parameter, bool toSystem = false)
    {
        if(toSystem == true) MainData.Set(parameter);
        else CurrentSave.Database.Set(parameter);
    }

    public GSC_Parameter Get(string key, bool toSystem = false)
    {
        if (toSystem == true) return MainData.Get(key);
        else return CurrentSave.Database.Get(key);
    }

    public string GetAsString(string key, bool toSystem = false)
    {
        if (toSystem == true) return MainData.GetAsString(key);
        else return CurrentSave.Database.GetAsString(key);
    }
}
