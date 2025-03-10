public class GSC_FloatOperator : GSC_Operator
{
    public GSC_NumericOperator Op;
    public float Value;

    public override void Operate() => GSC_DataManager.Instance.Operate(System, KeyToOperate, Op, Value);

}

