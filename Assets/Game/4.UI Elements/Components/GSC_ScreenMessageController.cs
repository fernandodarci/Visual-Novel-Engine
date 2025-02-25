using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ScreenMessageController : GSC_CanvasGroupController
{
    [Header("Visual Components")]
    [SerializeField] private string _ID;
    [SerializeField] private Image DialogueBackground;
    [SerializeField] private GSC_ScreenTextBuilder DialogueText;
   
    public bool IsFading => IsRunning;
    public bool IsBuilding => DialogueText.IsBuilding;

    public string ID => _ID;

    private void Start()
    {
        // Hide the dialogue panel by default
        Hide();
        ClearText();
    }
    
    public IEnumerator ShowMessagePanel(float duration, Func<bool> paused, Func<bool> ends)
    {
        if (!IsVisible)
        {
            yield return FadeIn(duration, paused, ends);
        }
    }

    public IEnumerator ShowMessage(string dialogue, float duration, bool append, 
        Func<bool> paused, Func<bool> ends)
    {
        yield return DialogueText.BuildText(dialogue, duration, append,paused,ends);
    }

    public IEnumerator HideMessage(float duration,Func<bool> paused, Func<bool> ends)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration, paused, ends);
        }
    }

    public void ClearText() => DialogueText.Clear();

}