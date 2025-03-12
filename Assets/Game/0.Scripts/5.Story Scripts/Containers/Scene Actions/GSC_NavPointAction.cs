using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_ClickableArea : GSC_PositionSize
{
    public string ValueIfClicked;

    public GSC_ClickableArea(Vector2 position, Vector2 size) : base(position, size)
    {
    }
}


public class GSC_NavPointAction : GSC_ScriptAction
{
    public List<GSC_ClickableArea> ClickableAreas;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit<GSC_ClickableArea[]> unit
            = new GSC_ContainerUnit<GSC_ClickableArea[]>("NavPoints");
        return unit;
    }

    public override bool Decompile(GSC_ContainerUnit unit)
    {
        if (unit is GSC_ContainerUnit<GSC_ClickableArea[]> @areas)
        {
            ClickableAreas = new List<GSC_ClickableArea>(areas.Get());
            return true;
        }
        return false;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit.Calling == "NavPoints" && unit is GSC_ContainerUnit<GSC_ClickableArea[]> @areas
            && !areas.Get().IsNullOrEmpty();
    }

    protected override IEnumerator InnerAction(Func<bool> paused, Func<bool> ends, Action onEnd)
    {
        var navPoints = GSC_GameManager.Instance.GetNavPoints();
        if (navPoints != null && Validate(Compile()))
        {
            navPoints.InitializeClickableArea(ClickableAreas);
            yield return navPoints.WaitForClick(paused, ends);
        }
        onEnd();
    }
}
