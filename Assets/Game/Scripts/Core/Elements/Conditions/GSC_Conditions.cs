using UnityEngine;

public enum GSC_NumericComparator 
{ 
    NONE, 
    EQUALS,
    NOT_EQUALS,
    GREATER,
    LESS,
    GREATER_EQUALS,
    LESS_EQUALS
}

public abstract class GSC_Conditions : MonoBehaviour
{
    public string KeyToCompare;
    public bool System;
    public abstract bool CompareValue();
}

public static class GSC_NumericComparatorExtensions
{
    public static GSC_NumericComparator FromString(this string str)
    {
        return str.Trim() switch
        {
            "==" => GSC_NumericComparator.EQUALS,
            "!=" => GSC_NumericComparator.NOT_EQUALS,
            ">" => GSC_NumericComparator.GREATER,
            "<" => GSC_NumericComparator.LESS,
            ">=" => GSC_NumericComparator.GREATER_EQUALS,
            "<=" => GSC_NumericComparator.LESS_EQUALS,
            _ => GSC_NumericComparator.NONE
        };
    }
}