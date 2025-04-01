using Unity.VisualScripting;
using UnityEngine;

public enum GSC_NumericOperator
{
    NONE, 
    SET, 
    ADD, 
    SUBTRACT,
    MULTIPLY,
    DIVIDE,
}

public abstract class GSC_Operator : MonoBehaviour
{
    public string KeyToOperate;
    public bool System;

    public abstract void Operate();
}

public static class GSC_NumericOperatorExtensions
{
    public static GSC_NumericOperator FromString(this string str)
    {
        return str.Trim() switch
        {
            "=" => GSC_NumericOperator.SET,
            "+" => GSC_NumericOperator.ADD,
            "-" => GSC_NumericOperator.SUBTRACT,
            "*" => GSC_NumericOperator.MULTIPLY,
            "/" => GSC_NumericOperator.DIVIDE,
            _ => GSC_NumericOperator.NONE,
        };
    }

    public static string ToString(this GSC_NumericOperator op)
    {
        return op switch
        {
            GSC_NumericOperator.NONE => string.Empty,
            GSC_NumericOperator.SET => "=",
            GSC_NumericOperator.ADD => "+",
            GSC_NumericOperator.SUBTRACT => "-",
            GSC_NumericOperator.MULTIPLY => "*",
            GSC_NumericOperator.DIVIDE => "/",
            _ => string.Empty,
        };
    }
}