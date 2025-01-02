using System;
using System.Collections.Generic;

[Serializable]
public class GSC_ContainerUnit
{
    public string Calling;
    private readonly Dictionary<string, bool> booleans;
    private readonly Dictionary<string, int> integers;
    private readonly Dictionary<string, float> floats;
    private readonly Dictionary<string, string> strings;

    public GSC_ContainerUnit(string calling)
    {
        Calling = calling;
        booleans = new Dictionary<string, bool>();
        integers = new Dictionary<string, int>();
        floats = new Dictionary<string, float>();
        strings = new Dictionary<string, string>();
    }

    public void Set(string key, bool value) => booleans[key] = value;
    public void Set(string key, int value) => integers[key] = value;
    public void Set(string key, float value) => floats[key] = value;
    public void Set(string key, string value) => strings[key] = value;

    public bool HasBoolean(string key) => booleans.ContainsKey(key);
    public bool HasInteger(string key) => booleans.ContainsKey(key);
    public bool HasFloat(string key) => booleans.ContainsKey(key);
    public bool HasString(string key) => booleans.ContainsKey(key);
    public bool GetBoolean(string key) => booleans[key];
    public int GetInteger(string key) => integers[key];
    public float GetFloat(string key) => floats[key];
    public string GetString(string key) => strings[key];

}

public class GSC_ContainerUnit<TParam> : GSC_ContainerUnit
{
    private TParam Asset;
    public bool IsValidAsset { get; private set; }

    public void Set(TParam asset)
    {
        Asset = asset;
        IsValidAsset = true;
    }

    public TParam Get() => Asset;

    public GSC_ContainerUnit(string calling) : base(calling) { }
}
