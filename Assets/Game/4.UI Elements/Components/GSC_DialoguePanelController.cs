using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_DialoguePanelController : GSC_CanvasGroupController
{
    private static string DIALOGUE_COMPLETE => "Press on screen to continue...";

    [SerializeField] private Image CharacterNameBackground;
    [SerializeField] private Image Separator;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private Image DialogueBackground;
    [SerializeField] private GSC_ScreenTextBuilder Dialogue;
    [SerializeField] private GSC_ScreenTextBuilder Completed;
    private bool EndDialogue;

    public void ChangeCharacterName(string name)
    {
        if (name.Trim().ToLower() == "narrator")
        {
            CharacterNameBackground.enabled = false;
            Separator.enabled = false;
        }
        else
        {
            CharacterNameBackground.enabled = true;
            Separator.enabled = true;
            CharacterName.text = name;
        }
    }

    public void ChangeCharacterNameColor(Color color)
    {
        CharacterName.color = color; 
    }

    public IEnumerator ShowDialoguePanel(float duration, Func<bool> pause, Func<bool> ends)
    {
        if (!IsVisible)
        {
            yield return FadeIn(duration,pause,ends);
        }
    }
    
    public IEnumerator HideDialoguePanel(float duration, Func<bool> pause, Func<bool> ends)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration, pause, ends);
        }
    }

    public IEnumerator ShowDialogue(string dialogue, float duration, bool append, Func<bool> pause, Func<bool> ends)
    {
        if(IsVisible)
        {
            EndDialogue = false;
            yield return Dialogue.BuildText(dialogue, duration, append, pause, ends);
            EndDialogue = true;
        }
    }

    public IEnumerator ShowCompleted(float duration, Func<bool> pause, Func<bool> ends)
    {
        if(IsVisible && EndDialogue)
        {
            yield return Completed.BuildText(DIALOGUE_COMPLETE, duration, false, pause, ends);
        }
    }

    public void ClearText()
    {
        Dialogue.Clear();
        Completed.Clear();
    }
}
