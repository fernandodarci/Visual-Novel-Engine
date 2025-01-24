using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Image Container")]
public class GSC_ShowImageContainerModule : GSC_ScriptContainer
{
    [SerializeField] private Sprite Frame;
    [SerializeField] private GSC_ImageControllers ControllerName;
    [SerializeField] private int Layer;
    [SerializeField] private float FadeTime;
    [SerializeField] private float HideAfterTime;
    
    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit<Sprite> unit = new("ShowImage");
        unit.Set(Frame);
        unit.Set("Controller",GSC_EnumToStringConvert.Get(ControllerName)); 
        unit.Set("Layer", Layer);
        unit.Set("Duration", FadeTime);
        unit.Set("HideAfter", HideAfterTime);

        return unit;
    }
}
