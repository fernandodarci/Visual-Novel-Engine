using System.Collections.Generic;

public class GSC_ContainerUnit
{
    public string Calling;
    private Dictionary<string, bool> booleanArgs;
    private Dictionary<string, int> integerArgs;
    private Dictionary<string, float> floatArgs;
    private Dictionary<string, string> stringArgs;

    public GSC_ContainerUnit(string calling)
    {
        Calling = calling;
        booleanArgs = new Dictionary<string, bool>();
        integerArgs = new Dictionary<string, int>();
        floatArgs = new Dictionary<string, float>();
        stringArgs = new Dictionary<string, string>();
    }

    public void Set(string name, bool value) => booleanArgs[name] = value;
    public void Set(string name, int value) => integerArgs[name] = value;
    public void Set(string name, float value) => floatArgs[name] = value;
    public void Set(string name, string value) => stringArgs[name] = value;

    public bool GetBoolean(string name) => booleanArgs.TryGetValue(name, out bool result) ? result : false;
    public int GetInteger(string name) => integerArgs.TryGetValue(name, out int result) ? result : 0;
    public float GetFloat(string name) => floatArgs.TryGetValue(name, out float result) ? result : 0f;
    public string GetString(string name) => stringArgs.TryGetValue(name, out var result) ? result : string.Empty;

    public bool HasBoolean(string name) => booleanArgs.ContainsKey(name);
    public bool HasInteger(string name) => integerArgs.ContainsKey(name);
    public bool HasFloat(string name) => floatArgs.ContainsKey(name);
    public bool HasString(string name) => stringArgs.ContainsKey(name);
}

public class GSC_ContainerUnit<T> : GSC_ContainerUnit
{
    private T Asset = default;
    private bool IsValidAsset = false;

    public GSC_ContainerUnit(string calling) : base(calling) { }
    
    public void Set(T asset)
    {
        Asset = asset;
        IsValidAsset = true;
    }
    public T Get() => Asset;
    public bool HasAsset() => IsValidAsset;
}

public enum GSC_ParameterType
{
    Boolean,
    Zero_or_Positive_Integer,
    Zero_or_Negative_Integer,
    NonZero_Positive_Integer,
    NonZero_Negative_Integer,
    Integer,
    Zero_or_Positive_Float,
    Zero_or_Negative_Float,
    NonZero_Positive_Float,
    NonZero_Negative_Float,
    Float,
    NonEmpty_String,
    String,
}

