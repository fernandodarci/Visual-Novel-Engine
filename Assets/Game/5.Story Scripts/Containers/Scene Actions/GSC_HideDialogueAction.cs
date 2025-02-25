using System;
using UnityEngine;

[Serializable]
public class GSC_HideDialogueAction : GSC_SceneAction
{
    public override string ID => "Hide Dialogue";
    [SerializeField] private float FadeTime;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("HideDialogue");
        unit.Set("Fade", FadeTime);
        return unit;
    }
}

