public class GSC_BooleanOperator : GSC_Operator
{
    public bool Set;
    public bool Value;

    public override void Operate() => GSC_DataManager.Instance.Operate(System, KeyToOperate, Set, Value);

}

