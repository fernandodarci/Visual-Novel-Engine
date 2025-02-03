using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Multiple Images Container")]
public class GSC_ShowImagesContainerModule : GSC_ScriptContainer
{
    [SerializeField] private GSC_SpriteLayer[] Frame;
    [SerializeField] private GSC_ImageControllers ControllerName;
    [SerializeField] private float FadeTime;
    

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit<GSC_SpriteLayer[]> unit = new("ShowImages");
        unit.Set(Frame);
        unit.Set("Controller", GSC_EnumToStringConvert.Get(ControllerName));
        unit.Set("Duration", FadeTime);
        
        return unit;
    }
}
