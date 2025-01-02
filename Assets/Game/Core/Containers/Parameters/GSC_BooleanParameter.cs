using System;

public class GSC_BooleanParameter : GSC_Parameter
{
    public GSC_BooleanParameter(string parameterName, bool value = default)
        : base(parameterName, GSC_ParameterType.Boolean) 
    { 
        Value = value;
    }

    public readonly bool Value;

    public GSC_BooleanParameter OnValid(GSC_Parameter parameter)
    {
        if (parameter is GSC_BooleanParameter b)
        {
            return new GSC_BooleanParameter(ParameterName, b.Value);
        }
        else return this;
    }
}
