using System;

public abstract class GSC_Parameter
{
    public readonly string ParameterName;
    public readonly GSC_ParameterType ParameterType;
   
    public GSC_Parameter(string parameterName, GSC_ParameterType parameterType)
    {
        ParameterName = parameterName;
        ParameterType = parameterType;
    }
}

public enum GSC_ParameterType
{
    Boolean, Integer, Float, String
}