using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GSC_ChangeBackgroundAction : GSC_ScriptAction
{
    public string GraphicName;
    public int Layer;
    public float FadeTime;
    public GSC_ChangeEffectType EffectType;
    public float WaitAfterFade;
    public bool HideAfterTime;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("ChangeBackground");
        unit.Set("GraphicName", GraphicName);
        unit.Set("Layer", Layer);
        unit.Set("FadeTime", FadeTime);
        unit.Set("EffectType", (int)EffectType);
        unit.Set("WaitAfterFade", WaitAfterFade);
        unit.Set("HideAfterTime", HideAfterTime);
        return unit;
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if (Validate(unit))
        {
            GraphicName = unit.GetString("GraphicName");
            Layer = unit.GetInteger("Layer");
            FadeTime = unit.GetFloat("FadeTime");
            EffectType = (GSC_ChangeEffectType)unit.GetInteger("EffectType");
            WaitAfterFade = unit.GetFloat("WaitAfterFade");
            HideAfterTime = unit.GetBoolean("HideAfterTime");
            return true;
        }
        Debug.LogError($"Invalid unit for ChangeBackground: {unit.Calling}");
        return false;
    }

    public override GSC_ContainerUnit TryDecodeScript(string[] line)
    {
        throw new NotImplementedException();
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "ChangeBackground" &&
               unit.HasString("GraphicName") && unit.HasInteger("Layer") && unit.HasFloat("FadeTime") &&
               unit.HasInteger("EffectType") && unit.HasFloat("WaitAfterFade") && unit.HasFloat("HideAfterTime");
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        Debug.Log("Inner Action of ChangeBackground");
        var Graphics = GSC_GraphicsManager.Instance;
        var image = Resources.Load<Sprite>(GraphicName);
        if (image != null)
        {
            GSC_SpriteContainer container = new GSC_SpriteContainer(image);
            yield return Graphics.ChangeScene(container, Layer, FadeTime, EffectType, paused, ends, onEnd);
            if (WaitAfterFade > 0) yield return GSC_Constants.WaitForSeconds(WaitAfterFade, paused, ends);
            if (HideAfterTime) yield return Graphics.HideLayer(Layer, FadeTime, paused, ends);
            Resources.UnloadAsset(image);
        }
        else
        {
            var texture = Resources.Load<Texture2D>(GraphicName);
            if(texture != null)
            {
                GSC_TextureContainer container = new GSC_TextureContainer(texture);
                yield return GSC_GraphicsManager.Instance
                    .ChangeScene(container, Layer, FadeTime, EffectType, paused, ends, onEnd);
                if (WaitAfterFade > 0) yield return GSC_Constants.WaitForSeconds(WaitAfterFade, paused, ends);
                if (HideAfterTime) yield return Graphics.HideLayer(Layer, FadeTime, paused, ends);
                Resources.UnloadAsset(image);
            }
            else Debug.LogError($"Graphic {GraphicName} not found.");
        }
        onEnd();
    }
}