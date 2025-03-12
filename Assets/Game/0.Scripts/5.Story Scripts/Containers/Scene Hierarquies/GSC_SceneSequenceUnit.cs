using System;
using System.Collections.Generic;

[Serializable]
public class GSC_SceneSequenceUnit
{
    public List<GSC_SequenceConditionsChain> Conditions;
    public List<GSC_SceneUnit> Scenes;

    public GSC_SceneSequenceUnit()
    {
        Conditions = new List<GSC_SequenceConditionsChain>();
        Scenes = new List<GSC_SceneUnit>();
    }

    public bool MetConditions()
    {
        if (Conditions.IsNullOrEmpty())
            return false;

        foreach (GSC_SequenceConditionsChain chain in Conditions)
        {
            if (chain.Match()) return true;
        }
        return false;
    }

}


