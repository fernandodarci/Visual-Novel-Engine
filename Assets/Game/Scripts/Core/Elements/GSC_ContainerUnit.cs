using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_ContainerUnit
{
    #region Fields

    public string Calling;
    private readonly Dictionary<string, bool> booleans;
    private readonly Dictionary<string, int> integers;
    private readonly Dictionary<string, float> floats;
    private readonly Dictionary<string, string> strings;

    public static object MainMenu { get; internal set; }

    #endregion

    #region Constructor

    public GSC_ContainerUnit(string calling)
    {
        Calling = calling;
        booleans = new Dictionary<string, bool>();
        integers = new Dictionary<string, int>();
        floats = new Dictionary<string, float>();
        strings = new Dictionary<string, string>();
    }

    #endregion

    #region Setters

    public void Set(string key, bool value) => booleans[key] = value;
    public void Set(string key, int value) => integers[key] = value;
    public void Set(string key, float value) => floats[key] = value;
    public void Set(string key, string value) => strings[key] = value;
    public void Switch(string key)
    {
        if(HasBoolean(key)) booleans[key] = !booleans[key];
    }


    #endregion

    #region Checkers

    public bool HasBoolean(string key) => booleans.ContainsKey(key);
    public bool HasInteger(string key) => integers.ContainsKey(key);
    public bool HasFloat(string key) => floats.ContainsKey(key);
    public bool HasString(string key) => strings.ContainsKey(key);
    
    #endregion
    
    #region Getters
    public bool GetBoolean(string key) => booleans[key];
    public int GetInteger(string key) => integers[key];
    public float GetFloat(string key) => floats[key];
    public string GetString(string key) => strings[key];

    #endregion
  
    #region Removal Methods

    public void RemoveBoolean(string key) => booleans.Remove(key);
    public void RemoveInteger(string key) => integers.Remove(key);
    public void RemoveFloat(string key) => floats.Remove(key);
    public void RemoveString(string key) => strings.Remove(key);

    #endregion
 
    #region Utility Methods

    public string GetAsString(string key)
    {
        if (HasBoolean(key))
            return booleans[key] ? "True" : "False";
        if (HasInteger(key))
            return integers[key].ToString();
        if (HasFloat(key))
            return floats[key].ToString("F3");
        if (HasString(key))
            return strings[key];
        return key;
    }

  
    #endregion

    #region Comparison Methods

    public bool CompareBool(string key, bool valueToCompare)
    {
        return booleans.ContainsKey(key) && booleans[key] == valueToCompare;
    }

    public bool CompareInteger(string key, GSC_NumericComparator comp, int valueToCompare)
    {
        if (!integers.ContainsKey(key)) return false;

        return comp switch
        {
            GSC_NumericComparator.EQUALS => integers[key] == valueToCompare,
            GSC_NumericComparator.NOT_EQUALS => integers[key] != valueToCompare,
            GSC_NumericComparator.GREATER => integers[key] > valueToCompare,
            GSC_NumericComparator.LESS => integers[key] < valueToCompare,
            GSC_NumericComparator.GREATER_EQUALS => integers[key] >= valueToCompare,
            GSC_NumericComparator.LESS_EQUALS => integers[key] <= valueToCompare,
            _ => false
        };
    }

    public bool CompareFloat(string key, GSC_NumericComparator comp, float valueToCompare)
    {
        if (!floats.ContainsKey(key))
            return false;

        float epsilon = 0.0001f;
        float value = floats[key];
        float diff = value - valueToCompare;

        return comp switch
        {
            GSC_NumericComparator.EQUALS => Mathf.Abs(diff) < epsilon,
            GSC_NumericComparator.NOT_EQUALS => Mathf.Abs(diff) >= epsilon,
            GSC_NumericComparator.GREATER => diff > epsilon,
            GSC_NumericComparator.LESS => -diff > epsilon,
            GSC_NumericComparator.GREATER_EQUALS => diff > -epsilon,
            GSC_NumericComparator.LESS_EQUALS => -diff > -epsilon,
            _ => false
        };
    }

    public bool CompareString(string key, string value)
    {
        if (!strings.ContainsKey(key))
        {
            Debug.Log("Key not found");
            return false;
        }
        if (strings[key] != value) Debug.Log($"Key is {value} but the register are for {strings[key]}");
        return strings.ContainsKey(key) && strings[key] == value;
    }

    #endregion

    #region JSON Serialization

    public string ToJson()
    {
        var result = new GSC_SerializableContainer(Calling);

        foreach (var kvp in booleans)
            result.Booleans.Add(new GSC_SerializableContainer.GSC_Boolean(kvp.Key, kvp.Value));
        foreach (var kvp in integers)
            result.Integers.Add(new GSC_SerializableContainer.GSC_Integer(kvp.Key, kvp.Value));
        foreach (var kvp in floats)
            result.Floats.Add(new GSC_SerializableContainer.GSC_Float(kvp.Key, kvp.Value));
        foreach (var kvp in strings)
            result.Strings.Add(new GSC_SerializableContainer.GSC_String(kvp.Key, kvp.Value));

        return JsonUtility.ToJson(result);
    }

    public static GSC_ContainerUnit FromJson(string json)
    {
        GSC_SerializableContainer container = JsonUtility.FromJson<GSC_SerializableContainer>(json);
        if (container == null) return new GSC_ContainerUnit("Null");

        GSC_ContainerUnit result = new GSC_ContainerUnit(container.Calling);
        foreach (var kvp in container.Booleans)
            result.Set(kvp.Label, kvp.Value);
        foreach (var kvp in container.Integers)
            result.Set(kvp.Label, kvp.Value);
        foreach (var kvp in container.Floats)
            result.Set(kvp.Label, kvp.Value);
        foreach (var kvp in container.Strings)
            result.Set(kvp.Label, kvp.Value);

        return result;
    }

    
    #endregion
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