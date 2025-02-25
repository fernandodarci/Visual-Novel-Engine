using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public abstract class GSC_Provider : ScriptableObject
{
    public abstract string[] GetNames();
    public abstract void Release(string name);

    public abstract void ReleaseAll();
}

public class GSC_Provider<T> : GSC_Provider where T : Object
{
    [SerializeField] private List<GSC_Asset<T>> Assets;

    public override string[] GetNames()
        => Assets.Select(x => x.Name).ToArray();

    public T LoadAsset(string name)
    {
        var assetEntry = Assets.Find(x => x.Name == name);
        if (assetEntry == null)
        {
            Debug.LogWarning($"Asset with name '{name}' not found.");
            return null;
        }

        if (assetEntry.Asset != null)
            return assetEntry.Asset;

        // Carrega o asset via Addressables de forma síncrona
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);
        assetEntry.Asset = handle.WaitForCompletion();
        return assetEntry.Asset;
    }

    public override void Release(string name)
    {
        var assetEntry = Assets.Find(x => x.Name == name);
        if (assetEntry != null && assetEntry.Asset != null)
        {
            Addressables.Release(assetEntry.Asset);
            assetEntry.Asset = null;
        }
    }

    public override void ReleaseAll()
    {
        if (Assets.Count > 0)
        {
            foreach (var assetEntry in Assets)
            {
                if (assetEntry.Asset != null)
                {
                    Addressables.Release(assetEntry.Asset);
                    assetEntry.Asset = null;
                }
            }
        }
    }
}

[Serializable]
public class GSC_Asset<T>
{
    public string Name;
    public T Asset;
}
