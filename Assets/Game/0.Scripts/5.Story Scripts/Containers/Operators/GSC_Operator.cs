using UnityEngine;

public enum GSC_NumericOperator
{
    SET, UNSET, ADD, SUBTRACT
}

public abstract class GSC_Operator : MonoBehaviour
{
    public string KeyToOperate;
    public bool System;

    public abstract void Operate();
}

