public class GSC_IntegerOperator : GSC_Operator
{
    public GSC_NumericOperator Op;
    public int Value;

    public override void Operate() => GSC_DataManager.Instance.Operate(System, KeyToOperate, Op, Value);

}

