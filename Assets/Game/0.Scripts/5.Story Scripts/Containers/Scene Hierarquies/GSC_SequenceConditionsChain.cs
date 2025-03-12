using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GSC_SequenceConditionsChain
{
    [SerializeReference] public List<GSC_Conditions> Conditions;

    public bool Match()
    {
        if (Conditions.IsNullOrEmpty())
            return false;

        foreach (var condition in Conditions)
        {
            if (!condition.CompareValue()) return false;
        }
        foreach (var condition in Conditions) GSC_DataManager.Instance.RemoveCondition(condition);
        return true;
    }
}


