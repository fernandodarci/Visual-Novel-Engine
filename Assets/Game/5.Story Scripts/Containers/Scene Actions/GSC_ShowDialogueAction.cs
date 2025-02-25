using System;
using UnityEngine;

[Serializable]
public class GSC_ShowDialogueAction : GSC_SceneAction
{
    public override string ID => $"Show Dialogue {CharacterName}";
    [SerializeField] private string CharacterName;
    [SerializeField][TextArea] private string Dialogue;
    [SerializeField] private bool Append;
    [SerializeField] private float Duration;
    [SerializeField] private float FadeTime;
    [SerializeField] private bool WaitToComplete;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowDialogue");
        unit.Set("Character", CharacterName);
        unit.Set("Dialogue", Dialogue);
        unit.Set("Append", Append);
        unit.Set("Duration", Duration);
        unit.Set("Fade", FadeTime);
        unit.Set("WaitToComplete", WaitToComplete);
        return unit;
    }

    
}
