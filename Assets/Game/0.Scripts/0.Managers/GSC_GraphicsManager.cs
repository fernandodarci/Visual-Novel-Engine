using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSC_GraphicsManager : GSC_Singleton<GSC_GraphicsManager>
{
    [SerializeField] private GSC_GraphicLayer FullscreenLayerPrefab;
    [SerializeField] private RectTransform Parent;
    private Dictionary<int, GSC_GraphicLayer> Layers = new Dictionary<int, GSC_GraphicLayer>();
    private List<GSC_GraphicLayer> LayersToChange = new List<GSC_GraphicLayer>();

    #region Layer Management

    private void OrderLayers()
    {
        foreach (int layer in Layers.Keys)
        {
            Layers[layer].GetComponent<RectTransform>().SetSiblingIndex(layer);
        }
    }

    public void AddLayer(int layer)
    {
        if (!Layers.ContainsKey(layer))
        {
            var image = Instantiate(FullscreenLayerPrefab, Parent);
            image.name = $"Layer [{layer}]";
            image.Initialize();
            image.Hide();
            Layers[layer] = image;
            OrderLayers();
        }
    }

    public void ChangeLayerPosition(int source, int target)
    {
        if (Layers.ContainsKey(source) && Layers.ContainsKey(target))
        {
            var temp = Layers[source];
            Layers[source] = Layers[target];
            Layers[target] = temp;
            OrderLayers();
        }
    }

    public void RemoveLayer(int layer)
    {
        if (Layers.ContainsKey(layer))
        {
            Destroy(Layers[layer].gameObject);
            Layers.Remove(layer);
        }
    }

    public void RemoveAll()
    {
        if (Layers.Count > 0)
        {
            foreach (var layer in Layers.Keys)
            {
                Destroy(Layers[layer].gameObject);
            }
            Layers.Clear();
        }
    }

    public void HideAllLayers()
    {
        if (Layers.Count > 0)
        {
            foreach (int layer in Layers.Keys) Layers[layer].Hide();
        }
    }

    public bool HasLayer(int layer)
    {
        return Layers.ContainsKey(layer);
    }

    #endregion

    #region Layer Operations

    public void SetSprite(int layer, Sprite sprite)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            value.SetSprite(sprite);
        }
    }

    public void SetColor(int layer, Color color)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            value.SetColor(color);
        }
    }

    public void SetMaterial(int layer, Material material)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            value.SetMaterial(material);
        }
    }

    #endregion

    public IEnumerator ChangeSprite(int layer, Sprite sprite, float duration, Func<bool> pause, Func<bool> end)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            yield return value.SmoothChangeSprite(sprite, duration, pause, end);
        }
    }

    public IEnumerator ChangeSprites(GSC_SpriteLayer[] layers, float duration)
    {
        foreach (GSC_SpriteLayer sprlayer in layers)
        {
            if (!Layers.TryGetValue(sprlayer.Layer, out GSC_GraphicLayer value))
            {
                AddLayer(sprlayer.Layer);
                value = Layers[sprlayer.Layer];
            }

            LayersToChange.Add(value);
            value.StartTransition(sprlayer.Sprite);
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            foreach (GSC_GraphicLayer layer in LayersToChange)
            {
                layer.SmoothChangeSpriteStep(elapsed / duration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (GSC_GraphicLayer layer in LayersToChange)
        {
            layer.EndTransition();
        }

        LayersToChange.Clear();
    }

    public IEnumerator ChangeColor(int layer, Color color, float duration, Func<bool> pause, Func<bool> end)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            yield return value.SmoothChangeColor(color, duration, pause, end);
        }
    }

    public IEnumerator HideLayer(int layer, float duration, Func<bool> pause, Func<bool> end)
    {
        if (Layers.TryGetValue(layer, out GSC_GraphicLayer value))
        {
            yield return value.FadeOut(duration, pause, end);
        }
    }

    internal bool TryGetAction(GSC_ContainerUnit unit, out GSC_CommandManager.CommandAction action)
    {
        throw new NotImplementedException();
    }
}

