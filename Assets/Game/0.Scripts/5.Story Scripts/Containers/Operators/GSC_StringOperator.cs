﻿public class GSC_StringOperator : GSC_Operator
{
    public bool Set;
    public string Value;

    public override void Operate() => GSC_DataManager.Instance.Operate(System,KeyToOperate, Set, Value);

}

