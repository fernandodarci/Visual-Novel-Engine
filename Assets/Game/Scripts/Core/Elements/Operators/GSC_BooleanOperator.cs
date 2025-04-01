public class GSC_BooleanOperator : GSC_Operator
{
    public bool Value;

    public override void Operate() => GSC_DataManager.Instance.Operate(System, KeyToOperate, Value);
}

public class GSC_SwitchOperator : GSC_Operator
{
    public override void Operate() => GSC_DataManager.Instance.Switch(System, KeyToOperate);
    
}