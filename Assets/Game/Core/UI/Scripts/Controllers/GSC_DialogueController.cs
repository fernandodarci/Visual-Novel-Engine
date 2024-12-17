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

    private void Start()
    {
        // Hide the dialogue panel by default
        Disable();
        DialogueText.Clear();
    }

    public IEnumerator ShowDialogue(GSC_ContainerUnit dialogue)
    {
        Enable(false);

        if(dialogue.HasString("Character"))
        {
            string characterName = dialogue.GetString("Character");
            if(characterName.Trim().ToLower() == "narrator")
            {
                CharacterNameBackground.enabled = false;
                Separator.enabled = false;
            }
            else
            {
                CharacterNameBackground.enabled = true;
                Separator.enabled = true;
                CharacterName.text = characterName;
            }
        }

        yield return DialogueText.BuildText(dialogue);
        
    }

    public void HideDialogue()
    {
        // Hide the dialogue panel
        Disable();
    }
}


