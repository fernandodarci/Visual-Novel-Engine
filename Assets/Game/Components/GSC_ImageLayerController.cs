using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSC_ImageLayerController : GSC_CanvasGroupController
{
    [SerializeField] private GSC_ImageController ImagePrefab;
    private Dictionary<int, GSC_ImageController> Layers = new Dictionary<int, GSC_ImageController>();
   
    private void OrderLayers()
    {
        foreach(int layer in Layers.Keys)
        {
            Layers[layer].GetComponent<RectTransform>().SetSiblingIndex(layer);
        }
    }

    public void AddLayer(int layer)
    {
        if (!Layers.ContainsKey(layer))
        {
            GSC_ImageController image = Instantiate(ImagePrefab,this.transform);
            image.name = $"Layer [{layer}]";
            OrderLayers();
            Layers[layer] = image;
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
        foreach(int layer in Layers.Keys) RemoveLayer(layer);
        Layers.Clear();
    }

    public void SetSprite(int layer, Sprite sprite)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            value.SetSprite(sprite);
        }
    }

    public void SetColor(int layer, Color color)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            value.SetColor(color);
        }
    }

    public void SetMaterial(int layer, Material material)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            value.SetMaterial(material);
        }
    }

    public void ToggleHighlight(int layer)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            value.ToggleHighlight();
        }
    }

    public void Move(int layer, Vector2 position)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            value.Move(position);
        }
    }

    public void Flip()
    {
        foreach (var layer in Layers.Values)
        {
            layer.Flip();
        }
    }

    public IEnumerator ChangeSprite(int layer, Sprite sprite, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.ChangeSprite(sprite, duration);
        }
    }

    public IEnumerator ChangeColor(int layer, Color color, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.ChangeColor(color, duration);
        }
    }

    public IEnumerator SmoothFlip(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.SmoothFlip(duration);
        }
    }

    public IEnumerator Moves(int layer, Vector2 position, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.Moves(position, duration);
        }
    }

    public IEnumerator Highlight(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.Highlight(duration);
        }
    }

    public IEnumerator Unhighlight(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.Unhighlight(duration);
        }
    }

    public IEnumerator SmoothMove(int layer, Vector2 targetPosition, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController value))
        {
            yield return value.SmoothMove(targetPosition, duration);
        }
    }
}

