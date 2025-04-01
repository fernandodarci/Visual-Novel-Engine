using System;
using System.Collections.Generic;

public static class GSC_SystemInfo
{

    private static Dictionary<string, Func<string>> SystemInfo = new()
    {
        { "Time", () => $"{DateTime.Now.Hour:D2}h{DateTime.Now.Minute:D2}m{DateTime.Now.Second:D2}s" },
    };
    
    public static string GetAsString(string key)
    {
        if (SystemInfo.ContainsKey(key))
            return SystemInfo[key]?.Invoke();
        return key;
    }
}