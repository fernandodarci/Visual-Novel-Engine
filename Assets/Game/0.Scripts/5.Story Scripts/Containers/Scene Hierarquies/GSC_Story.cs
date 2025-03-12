using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_Story : MonoBehaviour
{
    public List<GSC_SceneSequenceUnit> Sequences;

    public GSC_SceneSequenceUnit GetSequence()
    {
        foreach (GSC_SceneSequenceUnit sequence in Sequences)
        {
            if (sequence.MetConditions()) return sequence;
        }
        return null;
    }
}

