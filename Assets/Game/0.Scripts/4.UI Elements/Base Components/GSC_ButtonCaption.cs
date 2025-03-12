using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ButtonCaption : GSC_CanvasGroupController
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI caption;
    public Action<string> OnBtnClick;
    private string Value;

    public void Initialize(string value, string captionText = null)
    {
        Value = value;
        if (captionText == null) captionText = value;
        SetCaption(captionText);
        button.onClick.AddListener(() => OnBtnClick(Value));
    }
    public void SetImage(Sprite buttonImage)
    {
        button.image.sprite = buttonImage;
    }
    public void SetColor(Color color) => button.image.color = color;
    public void SetCaption(string captionText)
    {
        caption.text = captionText;
    }

    public void Freeze() => button.interactable = false;
    public void Unfreeze() => button.interactable = true;

    public string GetCaption() => Value;

    
}

