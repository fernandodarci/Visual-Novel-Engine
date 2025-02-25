using System;
using UnityEngine;

[Serializable]
public class GSC_ChangeBackgroundAction : GSC_SceneAction
{
    public override string ID => $"Change Background";

    [SerializeField] private string Group;
    [SerializeField] private string Frame;
    [SerializeField] private int Layer;
    [SerializeField] private float FadeTime;
    [SerializeField] private float WaitForSeconds;
    [SerializeField] private bool HideAfter;

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
}
