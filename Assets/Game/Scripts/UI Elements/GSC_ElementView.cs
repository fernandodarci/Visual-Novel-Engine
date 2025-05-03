using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class GSC_ElementView : MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    public bool IsFading { get; private set; }
    public bool IsVisible => _canvasGroup.alpha > 0;

    private void Awake()
    {
        ForceInitialize();
    }

    public void ForceInitialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn(float duration, bool enableOnEnd, Func<bool> pause, Func<bool> ends)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (ends()) break;

            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            }
            
            yield return null;
        }
        
        _canvasGroup.alpha = 1;
        
        if(enableOnEnd)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
    }

    public IEnumerator FadeOut(float duration, Func<bool> pause, Func<bool> ends)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (ends()) break;
            if (!pause())
            {
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            }

            yield return null;
        }
        Hide();
    }

    public void Show(bool interact = false)
    {
        ForceInitialize();
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = interact;
        _canvasGroup.blocksRaycasts = interact;
    }

    public void Hide()
    {
        ForceInitialize();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void SetVisibility(float value) => _canvasGroup.alpha = value;
    
}
