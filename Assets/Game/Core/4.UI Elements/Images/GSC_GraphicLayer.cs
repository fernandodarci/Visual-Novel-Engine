using System;
using System.Collections;
using UnityEngine;

public class GSC_GraphicLayer : MonoBehaviour
{
    [SerializeField] private GSC_ImageController Image;
    [SerializeField] private GSC_ImageController Overlay;

    public bool IsOverlayTransition { get; private set; }
    public Sprite BufferedSprite { get; private set; }

    public void Initialize()
    {
        Image.Hide();
        Overlay.Hide();
    }

    public void SetSprite(Sprite sprite) => Image.SetSprite(sprite);
    public void SetColor(Color color) => Image.SetColor(color);
    public void SetMaterial(Material material) => Image.SetMaterial(material);

    public void Show() => Image.Show();
    public void Hide() => Image.Hide();

    public IEnumerator FadeIn(float fadeTime)
        => Image.FadeIn(fadeTime);
    public IEnumerator FadeOut(float fadeTime)
        => Image.FadeOut(fadeTime);
    
    public IEnumerator SmoothChangeSprite(Sprite sprite, float fadeTime)
    {
        if(fadeTime < float.Epsilon)
        {
            SetSprite(sprite);
            Show();
            yield break;
        }

        if (!Image.IsVisible)
        {
            SetSprite(sprite);
            yield return FadeIn(fadeTime);
        }
        else
        {
            Overlay.SetSprite(sprite);
            
            float elapsed = 0f;

            while (elapsed < fadeTime)
            {
                Overlay.SetAlpha(Mathf.Lerp(0f, 1f, elapsed / fadeTime));
                Image.SetAlpha(Mathf.Lerp(1f, 0f, elapsed / fadeTime));
                elapsed += Time.deltaTime;
                yield return null;
            }

            Image.SetSprite(sprite);
            Overlay.Hide();
        }
        Image.Show();
    }

    public IEnumerator SmoothChangeColor(Color color, float duration)
        => Image.SmoothChangeColor(color, duration);
    
    //To multiple layers changing
    public void StartTransition(Sprite sprite)
    {
        if(!Image.IsVisible)
        {
            Image.SetSprite(sprite);
        }
        else
        {
            Overlay.SetSprite(sprite);
            Overlay.Hide();
            IsOverlayTransition = true;
            BufferedSprite = sprite;
        }
    }
    
    public void SmoothChangeSpriteStep(float step)
    {
        if(IsOverlayTransition)
        {
            Overlay.SetAlpha(Mathf.Lerp(0f, 1f, step));
            Image.SetAlpha(Mathf.Lerp(1f, 0f, step));
        }
        else Image.SetAlpha(step);
    }

    public void EndTransition()
    {
        if (IsOverlayTransition)
        {
            Image.SetSprite(BufferedSprite);
            Image.Show();
            Overlay.Hide();
        }
        else Image.Show();
    }
}


