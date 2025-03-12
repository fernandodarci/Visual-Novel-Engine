using System;
using System.Collections;

[Serializable]
public class GSC_PlaySoundAction : GSC_ScriptAction
{
    public override GSC_ContainerUnit Compile()
    {
        throw new NotImplementedException();
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        throw new NotImplementedException();
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        throw new NotImplementedException();
    }
}
