using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public abstract class GSC_Provider : ScriptableObject
{
    public abstract string[] GetNames { get; }
}

public abstract class GSC_Provider<T> : GSC_Provider where T : Object
{
    [SerializeField] protected List<GSC_AssetProviderData> Data;
    private AsyncOperationHandle handler;
    public override string[] GetNames => Data.Select(x => x.Name).ToArray();
    public T GetData(string Name)
    {
        if (string.IsNullOrEmpty(Name)) return null;
        GSC_AssetProviderData reference = Data.Find(x => x.Name.ToLower().Trim() == Name.ToLower().Trim());
        if (reference == null) return null;

        handler = Addressables.LoadAssetAsync<T>(reference.AssetData);
        handler.WaitForCompletion();
        if (handler.Status == AsyncOperationStatus.Succeeded)
        {
            if (handler.Result is T result)
            {
                return result;
            }
        }

        return null;
    }
}

[Serializable]
public class GSC_AssetProviderData
{
    public string Name;
    public AssetReference AssetData;
}
