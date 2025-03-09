using System;
using System.Collections;
using UnityEngine;

public class GSC_ImageCommandRegister 
{
    private static GSC_GraphicsManager Graphics => GSC_GraphicsManager.Instance;

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
    }



}