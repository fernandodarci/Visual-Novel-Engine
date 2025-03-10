using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_SceneSequenceUnit : MonoBehaviour
{
    public List<GSC_ConditionsChain> Conditions;
    public List<GSC_SceneUnit> Scenes;

    public bool MetConditions()
    {
        if (Conditions == null || Conditions.Count == 0)
            return false;

        foreach (GSC_ConditionsChain chain in Conditions)
        {
            if (chain.Match()) return true;
        }
        return false;
    }

}

[Serializable]
public class GSC_ConditionsChain
{
    [SerializeReference] public List<GSC_Conditions> Conditions;

    public bool Match()
    {
        if (Conditions == null || Conditions.Count == 0)
            return false;

        foreach (var condition in Conditions)
        {
            if (!condition.CompareValue()) return false;
        }
        foreach (var condition in Conditions) GSC_DataManager.Instance.RemoveCondition(condition);
        return true;
    }
}


