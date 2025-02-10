using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Show Image Script")]
public class GSC_ShowImageScript : GSC_ScriptContainer
{
    [SerializeField] private Sprite Frame;
    [SerializeField] private int Layer;
    [SerializeField] private GSC_ImageControllers ControllerName;
    [SerializeField] private float FadeTime;
    [SerializeField] private float HideAfter;

    public override GSC_ContainerUnit Compile(string name = null)
    {
        GSC_ContainerUnit<Sprite> unit = new("ShowImage");
        unit.Set(Frame);
        unit.Set("Layer",Layer);
        unit.Set("Controller", GSC_EnumToStringConvert.Get(ControllerName));
        unit.Set("Duration", FadeTime);
        unit.Set("HideAfter", HideAfter);
        return unit;
    }
}

[Serializable]
public class GSC_SpriteLayer
{
    public Sprite Sprite;
    public int Layer;
}