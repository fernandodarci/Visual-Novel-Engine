public class GSC_IntegerCondition : GSC_Conditions
{
    public GSC_NumericComparator IsValue;
    public int Value;

    public override bool CompareValue()
    {
        return GSC_DataManager.Instance.CompareInteger(System, KeyToCompare, IsValue, Value);
    }
}

