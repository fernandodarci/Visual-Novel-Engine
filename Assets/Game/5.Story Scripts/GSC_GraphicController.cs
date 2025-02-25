using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_GraphicController : GSC_CanvasGroupController
{
    [SerializeField] private Graphic _Image;

    public void SetSprite(Sprite sprite)
    {
        if (_Image is Image img) img.sprite = sprite;
        if (_Image is RawImage raw) raw.texture = sprite.texture;
    }

    public void SetColor(Color color) => _Image.color = color;
    public void SetMaterial(Material material) => _Image.material = material;
    public void Move(Vector2 position) => _Image.rectTransform.anchoredPosition = position;

    public IEnumerator SmoothChangeColor(Color color, float duration, Func<bool> pause, Func<bool> end)
    {
        Color originalcolor = _Image.color;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (end()) break;
            if (!pause())
            {
                _Image.color = Color.Lerp(originalcolor, color, elapsed / duration);
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
   
        SetColor(color);
    }
}


