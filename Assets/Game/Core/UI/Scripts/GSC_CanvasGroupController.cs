using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class GSC_CanvasGroupController : MonoBehaviour
{
    private CanvasGroup _CanvasGroup;

    public bool IsVisible => _CanvasGroup != null && Mathf.Approximately(_CanvasGroup.alpha, 1.0f);

    public bool IsRunning { get; private set; }

    private void Awake()
    {
        InitializeCanvasGroup();    
    }

    private void InitializeCanvasGroup()
    {
        // Ensure the CanvasGroup component exists
        _CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
    }

    public void Enable(bool blocksRaycast)
    {
        if (_CanvasGroup == null) InitializeCanvasGroup();

        _CanvasGroup.alpha = 1.0f;
        _CanvasGroup.blocksRaycasts = blocksRaycast;
        _CanvasGroup.interactable = true;
    }

    public void Disable()
    {
        if (_CanvasGroup == null) InitializeCanvasGroup();

        _CanvasGroup.alpha = 0.0f;
        _CanvasGroup.blocksRaycasts = false;
        _CanvasGroup.interactable = false;
    }

    #region Coroutines

    public IEnumerator Fade(float targetAlpha, float duration)
    {
        IsRunning = true;
        if (_CanvasGroup == null) InitializeCanvasGroup();
        
        float startAlpha = _CanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetAlpha(newAlpha);

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    public IEnumerator FadeIn(float duration, Func<bool> requestToEnd = null) => Fade(1f, duration);

    public IEnumerator FadeOut(float duration, Func<bool> requestToEnd = null) => Fade(0f, duration);

    public void SetAlpha(float alpha)
    {
        if (_CanvasGroup != null) InitializeCanvasGroup();
        _CanvasGroup.alpha = alpha;
    }

    #endregion
}
