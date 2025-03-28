public class GSC_BooleanCondition : GSC_Conditions
{
    public bool IsValue;
    public bool Value;

    public override bool CompareValue() => GSC_DataManager.Instance
        .CompareBoolean(System, KeyToCompare, IsValue, Value);
}

