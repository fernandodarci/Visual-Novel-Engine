using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_ContainerUnit
{
    public string Calling;
    private readonly Dictionary<string, bool> booleans;
    private readonly Dictionary<string, int> integers;
    private readonly Dictionary<string, float> floats;
    private readonly Dictionary<string, string> strings;

    public static object MainMenu { get; internal set; }

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
    public bool HasInteger(string key) => integers.ContainsKey(key);
    public bool HasFloat(string key) => floats.ContainsKey(key);
    public bool HasString(string key) => strings.ContainsKey(key);
    public bool GetBoolean(string key) => booleans[key];
    public int GetInteger(string key) => integers[key];
    public float GetFloat(string key) => floats[key];
    public string GetString(string key) => strings[key];

    public string ToJson()
    {
        var result = new GSC_SerializableContainer(Calling);
        foreach (var kvp in booleans) result.Booleans.Add
                (new GSC_SerializableContainer.GSC_Boolean(kvp.Key, kvp.Value));
        foreach (var kvp in integers) result.Integers.Add
                (new GSC_SerializableContainer.GSC_Integer(kvp.Key,kvp.Value));
        foreach(var kvp in floats) result.Floats.Add
                (new GSC_SerializableContainer.GSC_Float(kvp.Key,kvp.Value));
        foreach(var kvp in strings) result.Strings.Add
                (new GSC_SerializableContainer.GSC_String(kvp.Key,kvp.Value));
        return JsonUtility.ToJson(result);
    }

    public static GSC_ContainerUnit FromJson(string action)
    {
        GSC_SerializableContainer container = JsonUtility.FromJson< GSC_SerializableContainer>(action);
        if (container == null) return new GSC_ContainerUnit("Null");
        GSC_ContainerUnit result = new(container.Calling);
        foreach(var kvp in container.Booleans) result.Set(kvp.Label, kvp.Value);
        foreach(var kvp in container.Integers) result.Set(kvp.Label,kvp.Value);
        foreach (var kvp in container.Floats) result.Set(kvp.Label, kvp.Value);
        foreach (var kvp in container.Strings) result.Set(kvp.Label, kvp.Value);
        return result;
    }

    public string GetAsString(string value)
    {
        if(HasBoolean(value)) return booleans[value] ? "True" : "False";
        if(HasInteger(value)) return integers[value].ToString();
        if(HasFloat(value)) return floats[value].ToString("D3");
        if(HasString(value)) return strings[value].ToString();
        return value;
    }

    public void AddOrChange(GSC_ContainerUnit conditions)
    {
        foreach(var kvp in conditions.booleans)
            booleans[kvp.Key] = kvp.Value;
        foreach(var kvp in conditions.integers)
            integers[kvp.Key] = kvp.Value;
        foreach(var kvp in conditions.floats)
            floats[kvp.Key] = kvp.Value;
        foreach (var kvp in conditions.strings)
            strings[kvp.Key] = kvp.Value;
    }

    public bool Compare(GSC_ContainerUnit other)
    {
        foreach (var kvp in other.booleans)
             if (!booleans.TryGetValue(kvp.Key, out var result) || result != kvp.Value) return false;
        foreach (var kvp in other.integers)
            if (!integers.TryGetValue(kvp.Key, out var result) || result != kvp.Value) return false;
        foreach (var kvp in other.floats)
            if (!floats.TryGetValue(kvp.Key, out var result) || result != kvp.Value) return false;
        foreach (var kvp in other.strings)
            if (!strings.TryGetValue(kvp.Key, out var result) || result != kvp.Value) return false;
        return true;
    }
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

[Serializable]
public class GSC_SerializableContainer
{
    public string Calling;
    public List<GSC_Boolean> Booleans;
    public List<GSC_Integer> Integers;
    public List<GSC_Float> Floats;
    public List<GSC_String> Strings;

    public GSC_SerializableContainer(string calling)
    {
        Calling = calling;
        Booleans = new List<GSC_Boolean>();
        Integers = new List<GSC_Integer>();
        Floats = new List<GSC_Float>();
        Strings = new List<GSC_String>();
    }

    [Serializable]
    public class GSC_Boolean
    {
        public string Label;
        public bool Value;

        public GSC_Boolean(string label, bool value)
        {
            Label = label;
            Value = value;
        }
    }

    [Serializable]
    public class GSC_Integer
    {
        public string Label;
        public int Value;

        public GSC_Integer(string label, int value)
        {
            Label = label;
            Value = value;
        }
    }

    [Serializable]
    public class GSC_Float
    {
        public string Label;
        public float Value;

        public GSC_Float(string label, float value)
        {
            Label = label;
            Value = value;
        }
    }

    [Serializable]
    public class GSC_String
    {
        public string Label;
        public string Value;

        public GSC_String(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }

}