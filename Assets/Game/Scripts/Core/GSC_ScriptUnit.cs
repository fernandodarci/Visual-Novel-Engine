using System.Collections.Generic;

public class GSC_ScriptUnit
{
    public List<GSC_ScriptAction> Actions;
    public List<GSC_Operator> Operators;
    public void SetAllValues()
    {
        if(Operators.IsNullOrEmpty()) return;
        foreach (var op in Operators) op.Operate();
    }
}

