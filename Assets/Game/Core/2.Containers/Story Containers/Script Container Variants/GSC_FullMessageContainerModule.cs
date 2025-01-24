using UnityEngine;

[CreateAssetMenu(menuName = "Story Units/Full Message Container")]
public class GSC_FullMessageContainerModule : GSC_ScriptContainer
{
    [SerializeField] [TextArea(2,10)] private string Message;
    [SerializeField] private bool Append;
    [SerializeField] private float Duration;
    [SerializeField] private float FadeTime;
    [SerializeField] private bool WaitToComplete;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowMessage");

        unit.Set("MessageType", "FullMessage");
        unit.Set("Message", Message);
        unit.Set("Append", Append);
        unit.Set("Duration", Duration);
        unit.Set("Fade", FadeTime);
        unit.Set("WaitToComplete", WaitToComplete);

        return unit;
    }
}
