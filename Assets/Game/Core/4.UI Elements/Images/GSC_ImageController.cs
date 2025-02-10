using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ImageController : GSC_CanvasGroupController
{
    [SerializeField] private Image Image;

    public void SetSprite(Sprite sprite) => Image.sprite = sprite;
    public void SetColor(Color color) => Image.color = color;
    public void SetMaterial(Material material) => Image.material = material;
    public void Move(Vector2 position) => Image.rectTransform.anchoredPosition = position;

    public IEnumerator SmoothChangeColor(Color color, float duration)
    {
        Color originalcolor = Image.color;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Image.color = Color.Lerp(originalcolor, color, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetColor(color);
    }
}


