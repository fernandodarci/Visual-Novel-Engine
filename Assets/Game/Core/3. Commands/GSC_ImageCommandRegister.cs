using System;
using System.Collections;
using UnityEngine;

public class GSC_ImageCommandRegister : GSC_CommandRegister
{
    public new static void AddCommands(GSC_CommandDatabase command)
    {
        command.AddCommand("ShowImage", new Func<GSC_ContainerUnit, IEnumerator>(ShowImage))
            .With(new GSC_StringParameter("Controller",null,false))
            .With(GSC_IntegerParameter.WithRangeMin("Layer", 0))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 0))
            .With(GSC_FloatParameter.WithRangeMin("HideAfter", 0));
    }

    private static IEnumerator ShowImage(GSC_ContainerUnit unit)
    {
        Sprite sprite;
        if (unit is GSC_ContainerUnit<Sprite> @sprUnit)
        {
            sprite = sprUnit.Get();
            if (sprite != null)
            {
                GSC_ImageLayerController controller = Game.Controller(unit.GetString("Controller"));
                if (controller != null)
                {
                    int layer = unit.HasInteger("Layer") ? unit.GetInteger("Layer") : 0;
                    yield return controller.ChangeSprite(layer, sprite, unit.GetFloat("Duration"));
                    if (unit.GetFloat("HideAfter") > 0f)
                    {
                        yield return Game.WaitForSeconds(unit.GetFloat("HideAfter"));
                        yield return controller.HideLayer(layer, unit.GetFloat("Duration"));
                    }
                }
            }
        }
        Script.Ends();
    }
    
   

}