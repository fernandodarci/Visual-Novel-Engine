using System;
using System.Collections;
using UnityEngine;

public class GSC_ImageCommandRegister : GSC_CommandRegister
{
    private static GSC_GraphicsManager Graphics => GSC_GraphicsManager.Instance;

    public new static void AddCommands(GSC_CommandDatabase command)
    {
        command.AddCommand("ShowImage",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(ShowImage))
            .With(new GSC_StringParameter("Group", null, false))
            .With(new GSC_StringParameter("Frame", null, false))
            .With(GSC_IntegerParameter.WithRangeMin("Layer", 0))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 0))
            .With(GSC_FloatParameter.WithRangeMin("Wait", 0))
            .With(new GSC_BooleanParameter("Hide"));
    }

    private static IEnumerator ShowImage(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        string group = unit.GetString("Group");
        string frame = unit.GetString("Frame");
        Sprite sprite = GSC_ProviderManager.Instance.GetStoryFrame(group, frame);
        if (sprite != null)
        {
            int layer = unit.HasInteger("Layer") ? unit.GetInteger("Layer") : 0;
            if (!Graphics.HasLayer(layer)) Graphics.AddLayer(layer);
            yield return Graphics.ChangeSprite(layer, sprite, unit.GetFloat("Duration"), paused, ends);
            yield return GSC_Constants.WaitForSeconds(unit.GetFloat("Wait"), paused, ends);
            if(unit.GetBoolean("Hide"))
            yield return Graphics.HideLayer(layer, unit.GetFloat("Duration"), paused, ends);
        }
        Command.Ends();   
    }



}