using System;
using UnityEngine;

[Serializable]
public class GSC_HideDialogueAction : GSC_Action
{
    public new const string ID = "ID03";
    [SerializeField] private float FadeTime;
    public override string GetID() => ID;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("HideDialogue");
        unit.Set("Fade", FadeTime);
        return unit;
    }

    public override bool Decompile(string json)
    {
        GSC_ContainerUnit unit = GSC_ContainerUnit.FromJson(json);
        if (unit != null)
        {
            FadeTime = unit.GetFloat("Fade");
        }
        return false;
    }
}

