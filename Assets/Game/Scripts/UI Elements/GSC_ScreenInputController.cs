using System;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ScreenInputController : MonoBehaviour
{
    public Button Left;
    public Button Right;

    public void Initialize(Action onScreenInputLeft, Action onScreenInputRight)
    {
        Left.onClick.AddListener(() => onScreenInputLeft());
        Right.onClick.AddListener(() => onScreenInputRight());
    }
}
