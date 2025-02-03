using System;
using System.Collections;
using UnityEngine;

public class GSC_ImageCommandRegister : GSC_CommandRegister
{
    public new static void AddCommands(GSC_CommandDatabase command)
    {
        command.AddCommand("ShowImage", new Func<GSC_ContainerUnit, IEnumerator>(ShowImage))
            .With(GSC_IntegerParameter.WithRangeMin("Layer", 0))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 0));
        command.AddCommand("ShowImages", new Func<GSC_ContainerUnit, IEnumerator>(ShowImages))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 0));
    }

    private static IEnumerator ShowImage(GSC_ContainerUnit unit)
    {
        Sprite sprite;
        if (unit is GSC_ContainerUnit<Sprite> @sprUnit)
        {
            sprite = sprUnit.Get();
        }
        else
        {
            sprite = Game.GetSprite(unit.GetString("SpriteGroup"), unit.GetString("SpriteName"));
        }
        GSC_ImageLayerController controller = Game.Controller(unit.GetString("Controller"));
        if (!controller.IsVisible)
        {
            controller.HideAllLayers();
            controller.Enable(false);
        }

        int layer = unit.HasInteger("Layer") ? unit.GetInteger("Layer") : 0;
        yield return controller.ChangeSprite(layer, sprite, unit.GetFloat("Duration"));
        if (unit.HasFloat("HideAfter") && unit.GetFloat("HideAfter") > 0f)
        {
            yield return Game.WaitForSeconds(unit.GetFloat("HideAfter"));
            yield return controller.FadeOut(unit.GetFloat("Duration"));
        }
        Script.Ends();
    }
    
    private static IEnumerator ShowImages(GSC_ContainerUnit unit)
    {
        GSC_SpriteLayer[] sprites;
        if (unit is GSC_ContainerUnit<GSC_SpriteLayer[]> @sprUnit)
        {
            sprites = sprUnit.Get();


            GSC_ImageLayerController controller = Game.Controller(unit.GetString("Controller"));

            if (!controller.IsVisible)
            {
                controller.HideAllLayers();
                controller.Enable(false);
            }

            yield return controller.ChangeSprites(sprites, unit.GetFloat("Duration"));
            if (unit.HasFloat("HideAfter") && unit.GetFloat("HideAfter") > 0f)
            {
                yield return Game.WaitForSeconds(unit.GetFloat("HideAfter"));
                yield return controller.FadeOut(unit.GetFloat("Duration"));
            }
        }
        Script.Ends();
    }


}