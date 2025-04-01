using System.Collections.Generic;

public class GSC_Story
{
    public List<GSC_ScriptBlock> Sequences;
    private int CurrentSequence;

    //Logic for getting the next sequence in the story. Default implementation returns the next sequence.
    public GSC_ScriptBlock GetSequence()
    {
        CurrentSequence++;
        if (CurrentSequence >= Sequences.Count) return null;
        return Sequences[CurrentSequence];
    }
}
