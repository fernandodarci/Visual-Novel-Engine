using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_DialogueBox : GSC_ElementView
{
    private static string ON_COMPLETE_MESSAGE => "Press on screen to continue...";

    [SerializeField] private Image Background;
    [SerializeField] private Image CharacterNameBackground;
    [SerializeField] private Image Separator;
    [SerializeField] private Image DialogueBackground;
    [SerializeField] private TMP_Text CharacterName;
    [SerializeField] private GSC_SmoothTextRender Dialogue;
    [SerializeField] private GSC_SmoothTextRender EndDialogueMessage;
    
    private string LastCharacterName = string.Empty;

    public void SetCharacterName(string characterName, string nameToShow)
    {
        if (characterName.IsNullOrEmpty())
        {
            if (LastCharacterName != string.Empty)
            {
                CharacterName.text =
                    GSC_CharacterManager.Instance.GetCharacterName(LastCharacterName, nameToShow);
            }
        }
        else if (characterName.Trim().ToLower() == "narrator")
        {
            CharacterName.text = string.Empty;
            CharacterNameBackground.gameObject.SetActive(false);
            Separator.gameObject.SetActive(false);
        }
        else
        {
            LastCharacterName = characterName;
            CharacterName.text =
                GSC_CharacterManager.Instance.GetCharacterName(characterName, nameToShow);
            CharacterNameBackground.gameObject.SetActive(true);
            Separator.gameObject.SetActive(true);
        }
        
    }

    public IEnumerator SetDialogue(string dialogue, float duration, bool append, Func<bool> paused, Func<bool> ends)
    {
        yield return new WaitForSeconds(1f);
        dialogue = dialogue.Trim();
        yield return Dialogue.BuildText(dialogue, duration, append, paused, ends);
    }

    public IEnumerator ShowMessageToComplete(Func<bool> paused, Func<bool> ends)
    {
        yield return EndDialogueMessage.BuildText(ON_COMPLETE_MESSAGE,1f,false,paused,ends);
    }

    public void ClearMessages()
    {
        Dialogue.Clear();
        EndDialogueMessage.Clear();
    }
}

