using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ScreenMessageController : GSC_CanvasGroupController
{
    [Header("Visual Components")]
    [SerializeField] private Image DialogueBackground;
    [SerializeField] private GSC_ScreenTextBuilder DialogueText;
  
    public bool IsFading => IsRunning;
    public bool IsBuilding => DialogueText.IsBuilding;

    private void Start()
    {
        // Hide the dialogue panel by default
        Hide();
        ClearText();
    }
    
    public IEnumerator ShowMessagePanel(float duration)
    {
        if (!IsVisible)
        {
            yield return FadeIn(duration);
        }
    }

    public IEnumerator ShowMessage(string dialogue, float duration, bool append = false)
    {
        yield return DialogueText.BuildText(dialogue, duration, append);
    }

    public IEnumerator HideDialogue(float duration)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration);
        }
    }

    public void CompleteDialogue() => DialogueText.CompleteDialogue();

    public void ClearText() => DialogueText.Clear();

}