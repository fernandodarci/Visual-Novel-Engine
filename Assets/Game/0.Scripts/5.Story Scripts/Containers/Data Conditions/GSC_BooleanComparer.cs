public class GSC_BooleanComparer : GSC_ValueComparer
{
    public enum GSC_BooleanComparation
    {
        AND, OR, XOR
    }

    public GSC_BooleanComparation Comparation;
    public bool ValueToCompare;

    public override bool Compare()
    {
        if(GSC_DataManager.Instance.GetBooleanValue(KeyToCompare, System, out bool value))
        {
            return Comparation switch
            {
                GSC_BooleanComparation.AND => value && ValueToCompare,
                GSC_BooleanComparation.OR => value || ValueToCompare,
                GSC_BooleanComparation.XOR => value != ValueToCompare,
                _ => false, //Never must happen
            };
        }
        return false;
    }
}
