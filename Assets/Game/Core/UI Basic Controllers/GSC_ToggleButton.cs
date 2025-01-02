using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ToggleButton : GSC_CanvasGroupController
{
    [SerializeField] private Button Button;
    [SerializeField] private TextMeshProUGUI Caption;
    public Action OnFalse;
    public Action OnTrue;
    public Action<bool> OnChange;
    public bool Value { get; private set; }

    public void Initialize(string caption, bool value, bool silent = false)
    {
        Value = value;
        Caption.text = caption;
        OnChange?.Invoke(Value);
    }

    public void SetCaption(string caption) => Caption.text = caption;
    public void SetColor(Color color) => Button.image.color = color;

    public void Toggle()
    {
        Value = !Value;
        if(Value == true) OnTrue?.Invoke();
        else OnFalse?.Invoke();

        OnChange?.Invoke(Value);
    }
}

public class GSC_ToggleTextChanger : MonoBehaviour
{
    [SerializeField] public string OnTrue;
    [SerializeField] public string OnFalse;
    [SerializeField] public GSC_ToggleButton Toggle;

    public void Initialize()
    {
        Toggle.OnTrue += OnToggleTrue;
        Toggle.OnFalse += OnToggleFalse;
        Toggle.Initialize(OnFalse, false);
    }

    private void OnToggleFalse()
    {
        Toggle.SetCaption(OnFalse);
    }

    private void OnToggleTrue()
    {
        Toggle.SetCaption(OnTrue);
    }
}