using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_SceneUnit
{
    [SerializeReference] public List<GSC_ScriptAction> Actions;
    [SerializeReference] public List<GSC_Operator> ValueSetters;

    public GSC_SceneUnit()
    {
        Actions = new List<GSC_ScriptAction>();
        ValueSetters = new List<GSC_Operator>();
    }

    public void AddValueToSet<T>(T value) where T : GSC_Operator
    {
        ValueSetters.Add(value);
    }

    public void SetActionReference<T>(T action) where T : GSC_ScriptAction
    {
        Actions.Add(action);
    }

    public void SetAllValues()
    {
        if (ValueSetters.IsNullOrEmpty()) return;
        foreach(var op in ValueSetters) op.Operate();
    }
}

