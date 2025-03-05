using System;
using UnityEngine;

public class GSC_ChangeBackgroundAction : GSC_SceneAction
{
    public new const string ID = "ID01";

    [SerializeField] private string Group;
    [SerializeField] private string Frame;
    [SerializeField] private int Layer;
    [SerializeField] private float FadeTime;
    [SerializeField] private float WaitForSeconds;
    [SerializeField] private bool HideAfter;

    public override string GetID() => ID;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowImage");
        unit.Set("Group", Group);
        unit.Set("Frame", Frame);
        unit.Set("Layer", Layer);
        unit.Set("Duration", FadeTime);
        unit.Set("Wait", WaitForSeconds);
        unit.Set("Hide", HideAfter);
        return unit;
    }

    public override void Decompile(string json)
    {
        GSC_ContainerUnit result = GetContainer(json);
        if (result != null)
        {
            Group = result.GetString("Group");
            Frame = result.GetString("Frame");
            Layer = result.GetInteger("Layer");
            FadeTime = result.GetFloat("FadeTime");
            WaitForSeconds = result.GetFloat("Wait");
            HideAfter = result.GetBoolean("HideAfter");
        }
    }
}
