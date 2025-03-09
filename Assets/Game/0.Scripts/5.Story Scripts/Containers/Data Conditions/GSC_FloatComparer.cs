using UnityEngine;

public class GSC_FloatComparer : GSC_ValueComparer
{
    private const float Precision = 0.0001f;

    public enum GSC_FloatComparation
    {
        EQUALS, NOT_EQUALS, GREATER_THAN, LESS_THAN, GREATER_THAN_OR_EQUAL, LESS_THAN_OR_EQUAL
    }

    public GSC_FloatComparation Comparation;
    public float ValueToCompare;

    private bool _equal(float a, float b) => Mathf.Abs(a - b) < Precision;
    private bool _greater(float a, float b) => (a - Precision) > (b + Precision);
    private bool _less(float a, float b) => (a + Precision) < (b - Precision);
    

    public override bool Compare()
    {
        if (GSC_DataManager.Instance.GetFloatValue(KeyToCompare, System, out float value))
        {
            return Comparation switch
            {
                GSC_FloatComparation.EQUALS => _equal(value,ValueToCompare),
                GSC_FloatComparation.NOT_EQUALS => !_equal(value,ValueToCompare),
                GSC_FloatComparation.GREATER_THAN => _greater(value,ValueToCompare),
                GSC_FloatComparation.LESS_THAN => _less(value,ValueToCompare),
                GSC_FloatComparation.GREATER_THAN_OR_EQUAL 
                => _equal(value,ValueToCompare) || _greater(value,ValueToCompare),
                GSC_FloatComparation.LESS_THAN_OR_EQUAL 
                => _equal(value,ValueToCompare) || _less(value,ValueToCompare),
                _ => false, //Never must happen
            };
        }
        return false;
    }
}
