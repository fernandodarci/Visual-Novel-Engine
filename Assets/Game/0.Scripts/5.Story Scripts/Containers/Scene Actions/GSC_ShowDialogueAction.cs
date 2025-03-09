using UnityEngine;

public class GSC_ShowDialogueAction : GSC_Action
{
    public new const string ID = "ID02";
    public override string GetID() => ID;

    public GSC_ShowDialogueData data;

    public override GSC_ContainerUnit Compile()
    {
        return data.Compile();
    }

    public override bool Decompile(string json)
    {
        data = new GSC_ShowDialogueData();
        return data.Decompile(json);
    }
}

