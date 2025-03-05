using UnityEngine;

public class GSC_ShowFullMessageAction : GSC_SceneAction
{
    public new const string ID = "ID04";

    [SerializeField][TextArea(2, 10)] private string Message;
    [SerializeField] private bool Append;
    [SerializeField] private float Duration;
    [SerializeField] private float FadeTime;
    [SerializeField] private bool WaitToComplete;
    public override string GetID() => ID;

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

    public override void Decompile(string json)
    {
        GSC_ContainerUnit result = GetContainer(json);
        if (result != null)
        {
            Message = result.GetString("Message");
            Append = result.GetBoolean("Append");
            Duration = result.GetFloat("Duration");
            FadeTime = result.GetFloat("Fade");
            WaitToComplete = result.GetBoolean("WaitToComplete");
        }
    }

}
