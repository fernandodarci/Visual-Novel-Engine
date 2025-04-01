public class GSC_FloatCondition : GSC_Conditions
{
    public GSC_NumericComparator IsValue;
    public float Value;

    public override bool CompareValue()
    {
       return GSC_DataManager.Instance.CompareFloat(System, KeyToCompare, IsValue, Value);
    }
}

