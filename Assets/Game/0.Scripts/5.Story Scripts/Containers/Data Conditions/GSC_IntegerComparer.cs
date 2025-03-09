public class GSC_IntegerComparer : GSC_ValueComparer
{
    public enum GSC_IntegerComparation
    {
        EQUALS, NOT_EQUALS, GREATER_THAN, LESS_THAN, GREATER_THAN_OR_EQUAL, LESS_THAN_OR_EQUAL
    }

    public GSC_IntegerComparation Comparation;
    public int ValueToCompare;

    public override bool Compare()
    {
        if (GSC_DataManager.Instance.GetIntegerValue(KeyToCompare, System, out int value))
        {
            return Comparation switch
            {
                GSC_IntegerComparation.EQUALS => value == ValueToCompare,
                GSC_IntegerComparation.NOT_EQUALS => value != ValueToCompare,
                GSC_IntegerComparation.GREATER_THAN => value > ValueToCompare,
                GSC_IntegerComparation.LESS_THAN => value < ValueToCompare,
                GSC_IntegerComparation.GREATER_THAN_OR_EQUAL => value >= ValueToCompare,
                GSC_IntegerComparation.LESS_THAN_OR_EQUAL => value <= ValueToCompare,
                _ => false, //Never must happen
            };
        }
        return false;
    }
}
