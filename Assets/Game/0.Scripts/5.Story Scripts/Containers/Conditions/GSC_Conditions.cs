using UnityEngine;

public enum GSC_NumericComparator { Equals, Not_Equals, Greater, Less, Greater_Equals, Less_Equals }

public abstract class GSC_Conditions : MonoBehaviour
{
    public string KeyToCompare;
    public bool System;
    public abstract bool CompareValue();
}

