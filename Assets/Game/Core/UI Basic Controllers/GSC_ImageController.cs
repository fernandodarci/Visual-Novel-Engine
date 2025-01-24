using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ImageController : GSC_CanvasGroupController
{
    [SerializeField] private Image image;
    private float _highlightFactor => 0.65f;
    private Color _highlightColor;
    private Color _originalColor;
    private Color _bufferColor;  // Armazenar a cor atual para transições corretas
    private bool IsHighlighted;
    private bool IsFlipped;
    private bool IsChanging;
    private bool RequestToComplete;
    public void SetSprite(Sprite sprite) => image.sprite = sprite;

    public void SetColor(Color color)
    {
        _originalColor = color;
        _highlightColor = color * new Color(_highlightFactor, _highlightFactor, _highlightFactor, 1f);
        _bufferColor = image.color; // Armazenar o valor da cor atual
        image.color = IsHighlighted ? _originalColor : _highlightColor;
    }

    public void SetMaterial(Material material) => image.material = material;

    public void ToggleHighlight()
    {
        IsHighlighted = !IsHighlighted;
        image.color = IsHighlighted ? _originalColor : _highlightColor;
    }

    public void Move(Vector2 position)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }
    }

    public void Flip()
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = IsFlipped ? new Vector3(-1, 1, 1) : Vector3.one;
            IsFlipped = !IsFlipped;
        }
    }

    public IEnumerator ChangeSprite(Sprite sprite, float duration)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration);
        }
        image.sprite = sprite;
        yield return FadeIn(duration);
        yield break;
    }

    public IEnumerator ChangeColor(Color color, float duration)
    {
        IsChanging = true;
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if(RequestToComplete)
            { 
                RequestToComplete = false;
                break;
            }
            image.color = Color.Lerp(startColor, color, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetColor(color);
        IsChanging = false;
    }

    public IEnumerator SmoothFlip(float duration)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration);
        }
        Flip();
        yield return FadeIn(duration);
    }

    public IEnumerator Moves(Vector2 position, float duration)
    {
        IsChanging = true;
        Vector2 originalPosition = image.rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if(RequestToComplete)
            {
                RequestToComplete = false; 
                break;
            }
            image.rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.rectTransform.anchoredPosition = position;
        IsChanging = false;
    }

    public IEnumerator Highlight(float duration)
    {
        IsChanging = true;
        Color startColor = _bufferColor; // Usar o buffer para garantir a transição correta
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (RequestToComplete)
            {
                RequestToComplete = false;
                break;
            }
            image.color = Color.Lerp(startColor, _originalColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = _originalColor;
        IsHighlighted = true;
        IsChanging = false;
    }

    public IEnumerator Unhighlight(float duration)
    {
        IsChanging = true;
        Color startColor = _bufferColor; // Usar o buffer para garantir a transição correta
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if(RequestToComplete)
            {
                RequestToComplete = false; 
                break;
            }
            image.color = Color.Lerp(startColor, _highlightColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = _highlightColor;
        IsHighlighted = false;
        IsChanging = false;
    }

    public IEnumerator SmoothMove(Vector2 targetPosition, float duration)
    {
        IsChanging = true;
        Vector2 startPosition = image.rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (RequestToComplete)
            {
                RequestToComplete = false;
                break;
            }
            image.rectTransform.anchoredPosition
                = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.rectTransform.anchoredPosition = targetPosition;
        RequestToComplete = false;
    }

    public void RequestEnd() => RequestToComplete = true;
}

