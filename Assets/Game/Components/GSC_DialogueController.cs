using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_DialogueController : GSC_CanvasGroupController
{
    [Header("Visual Components")]
    [SerializeField] private Image DialoguePanelBackground;
    [SerializeField] private Image CharacterNameBackground;
    [SerializeField] private Image Separator;
    [SerializeField] private Image DialogueBackground;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private GSC_ScreenTextBuilder DialogueText;
    [SerializeField] private GSC_ScreenTextBuilder DialogueEnd;

    private void Start()
    {
        // Hide the dialogue panel by default
        Disable();
        DialogueText.Clear();
        DialogueEnd.Clear();
    }

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


    public IEnumerator ShowDialoguePanel(float duration)
    {
        if(!IsVisible)
        {
            yield return FadeIn(duration); 
        }
    }

    public IEnumerator ShowDialogue(string dialogue, float duration, bool append = false)
    { 
        yield return DialogueText.BuildText(dialogue,duration,append);
    }

    public IEnumerator ShowAlert()
    {
        yield return DialogueEnd.BuildText("Press on screen to continue...", 0.5f, false);
    }

    public IEnumerator HideDialogue(float duration)
    {
        if (IsVisible)
        {
            yield return FadeOut(duration);
        }
    }

    public void ClearAlert() => DialogueEnd.Clear();

    public void CompleteDialogue() => DialogueText.CompleteDialogue();
    
}


