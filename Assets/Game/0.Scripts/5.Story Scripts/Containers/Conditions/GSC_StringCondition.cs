public class GSC_StringCondition : GSC_Conditions
{
    public bool IsValue;
    public string Value;

    public override bool CompareValue() => GSC_DataManager.Instance
      .CompareString(System, KeyToCompare, IsValue, Value);

}

