using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GSC_ChangeBackgroundAction : GSC_ScriptAction
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
        unit.Set("FadeTime", FadeTime);
        unit.Set("Wait", WaitForSeconds);
        unit.Set("HideAfter", HideAfter);
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "ShowImage" && unit.HasString("Group") && unit.HasString("Frame")
            && unit.HasInteger("Layer") && unit.HasFloat("FadeTime") && unit.HasFloat("Wait") 
            && unit.HasBoolean("HideAfter");
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

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        Sprite sprite = GSC_ProviderManager.Instance.GetStoryFrame(Group, Frame);
        GSC_GraphicsManager manager = GSC_GraphicsManager.Instance;
        if (sprite != null && Validate(Compile()))
        {
            if (!manager.HasLayer(Layer)) manager.AddLayer(Layer);
            yield return manager.ChangeSprite(Layer, sprite, FadeTime, paused, ends);
            yield return GSC_Constants.WaitForSeconds(WaitForSeconds, paused, ends);
            if (HideAfter == true)
                yield return manager.HideLayer(Layer, FadeTime, paused, ends);
        }
        onEnd();
    }
}
