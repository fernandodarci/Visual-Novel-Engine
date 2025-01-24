using System.Collections;
using UnityEngine;

public static class GSC_MonoBehaviourUtils
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if(component == null) go.AddComponent<T>();
        return component;
    }
}

public static class GSC_EnumToStringConvert
{
    public static string Get(GSC_ImageControllers controller)
    {
        return controller switch
        {
            GSC_ImageControllers.BACKGROUND => "Background",
            GSC_ImageControllers.CHARACTERS => "Characters",
            GSC_ImageControllers.FOREGROUND => "Foreground",
            _ => string.Empty
        };
    }

    public static GSC_ImageControllers Get(string controller)
    {
        return controller switch
        {
            "Background" => GSC_ImageControllers.BACKGROUND,
            "Characters" => GSC_ImageControllers.CHARACTERS,
            "Foreground" => GSC_ImageControllers.FOREGROUND,
            _ => default
        };

    }
}