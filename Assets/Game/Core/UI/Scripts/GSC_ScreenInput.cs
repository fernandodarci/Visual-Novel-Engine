using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ScreenInput : GSC_CanvasGroupController
{
    [SerializeField] private Button Left;
    [SerializeField] private Button Right;

    // Register callback actions for the left and right buttons
    public void RegisterEvent(Action onLeftButtonPressed, Action onRightButtonPressed)
    {
        // Remove existing listeners to avoid duplication
        Left.onClick.RemoveAllListeners();
        Right.onClick.RemoveAllListeners();

        // Add the new listeners
        Left.onClick.AddListener(() => onLeftButtonPressed?.Invoke());
        Right.onClick.AddListener(() => onRightButtonPressed?.Invoke());
    }
}

