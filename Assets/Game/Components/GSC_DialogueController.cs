using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GSC_DialogueController : GSC_ScreenMessageController
{
    [Header("Character")]
    [SerializeField] private Image CharacterNameBackground;
    [SerializeField] private Image Separator;
    [SerializeField] private TextMeshProUGUI CharacterName;
    
    
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

    public IEnumerator ShowDialoguePanel(float duration)
    {
        if (!IsVisible)
        {
            yield return FadeIn(duration);
        }
    }
}
