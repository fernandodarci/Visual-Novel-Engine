public class GSC_StringComparer : GSC_ValueComparer
{
    public bool IsValue;
    public string ValueToCompare;

    public override bool Compare()
    {
        if (GSC_DataManager.Instance.GetStringValue(KeyToCompare, System, out string value)
            && !string.IsNullOrWhiteSpace(ValueToCompare))
        {
            string comparedValue = ValueToCompare.Trim();
            return IsValue ? value.Equals(comparedValue) : !value.Equals(comparedValue);
        }
        return false;
    }

}
