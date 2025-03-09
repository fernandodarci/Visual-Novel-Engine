using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_SceneSequenceUnit : MonoBehaviour
{
    public GSC_SequenceCondition ConditionToRun;
    public List<GSC_SceneUnit> Scenes;

    public bool MetConditions() => ConditionToRun.Match();
    
}

[Serializable]
public class GSC_SequenceCondition
{
    [SerializeReference] public List<GSC_ValueComparer> Condition;

    public bool Match()
    {
        if (Condition == null || Condition.Count == 0) return false;
        foreach(var condition in Condition)
        {
            if (condition == null || !condition.Compare()) return false;
        }
        return true;
        
    }
}
