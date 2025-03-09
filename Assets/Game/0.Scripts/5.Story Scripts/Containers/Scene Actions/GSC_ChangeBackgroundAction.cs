using System;

public class GSC_ChangeBackgroundAction : GSC_Action
{
    public new const string ID = "ID01";
    public override string GetID() => ID;
    public GSC_ChangeBackgroundData Data;
    
}

[Serializable]
public class GSC_ChangeBackgroundData : GSC_ScriptData
{
    public string Group;
    public string Frame;
    public int Layer;
    public float FadeTime;
    public float WaitForSeconds;
    public bool HideAfter;

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

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "ShowImage" && unit.HasString("Group") && unit.HasString("Frame")
            && unit.HasInteger("Layer") && unit.HasFloat("Wait") && unit.HasFloat("Hide");
    }

    public override bool Decompile(GSC_ContainerUnit result)
    {
        if (result != null && Validate(result))
        {
            Group = result.GetString("Group");
            Frame = result.GetString("Frame");
            Layer = result.GetInteger("Layer");
            FadeTime = result.GetFloat("FadeTime");
            WaitForSeconds = result.GetFloat("Wait");
            HideAfter = result.GetBoolean("HideAfter");
            return true;
        }
        else return false;
    }


}
