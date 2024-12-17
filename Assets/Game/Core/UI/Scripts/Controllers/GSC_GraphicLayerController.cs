using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSC_GraphicLayerController : GSC_CanvasGroupController
{
    private Dictionary<int, GSC_ImageController> Layers = new Dictionary<int, GSC_ImageController>();
    [SerializeField] private GSC_ImageController prefab;

    #region Layer
    public void AddLayer(int layer, bool showOnCreate)
    {
        if (Layers.ContainsKey(layer)) return;

        GSC_ImageController newLayer = Instantiate(prefab, transform);
        newLayer.gameObject.SetActive(true);
        if (showOnCreate) newLayer.Enable(true);
        else newLayer.Disable();

        newLayer.transform.SetSiblingIndex(layer);
        Layers.Add(layer, newLayer);
    }

    public void RemoveLayer(int layer)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerToRemove))
        {
            Destroy(layerToRemove.gameObject);
            Layers.Remove(layer);
        }
    }

    public void DestroyAllLayers()
    {
        foreach (var layer in Layers.Values)
        {
            Destroy(layer.gameObject);
        }
        Layers.Clear();
    }
    #endregion

    #region Image
    public void SetSprite(int layer, Sprite sprite)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.SetSprite(sprite);
        }
    }

    public void SetColor(int layer, Color color)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.SetColor(color);
        }
    }

    public void SetMaterial(int layer, Material material)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.SetMaterial(material);
        }
    }

    public void ToggleHighlight(int layer)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.ToggleHighlight();
        }
    }

    public void Move(int layer, Vector2 position)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.Move(position);
        }
    }

    public void Flip(int layer)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            layerController.Flip();
        }
    }

    public IEnumerator ChangeSprite(int layer, Sprite sprite, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.ChangeSprite(sprite, duration);
        }
    }

    public IEnumerator ChangeColor(int layer, Color color, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.ChangeColor(color, duration);
        }
    }

    public IEnumerator Flips(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.SmoothFlip(duration);
        }
    }

    public IEnumerator Moves(int layer, Vector2 position, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.Moves(position, duration);
        }
    }

    public IEnumerator Highlight(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.Highlight(duration);
        }
    }

    public IEnumerator Unhighlight(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.Unhighlight(duration);
        }
    }

    public IEnumerator SmoothMove(int layer, Vector2 position, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.SmoothMove(position, duration);
        }
    }

    public IEnumerator SmoothFlip(int layer, float duration)
    {
        if (Layers.TryGetValue(layer, out GSC_ImageController layerController))
        {
            yield return layerController.SmoothFlip(duration);
        }
    }
    #endregion
}
