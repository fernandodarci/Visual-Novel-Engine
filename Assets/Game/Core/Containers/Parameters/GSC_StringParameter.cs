using System;

public class GSC_StringParameter : GSC_Parameter
{
    public GSC_StringParameter(string parameterName, string value = null, bool canBeEmpty = true)
        : base(parameterName, GSC_ParameterType.String) 
    { 
        CanBeEmpty = canBeEmpty;
        Value = value;
    }

    public readonly bool CanBeEmpty;
    public readonly string Value;

    public GSC_StringParameter OnValid(GSC_Parameter parameter)
    {
        if(parameter is GSC_StringParameter s && (CanBeEmpty || !string.IsNullOrEmpty(s.Value)))
            return new GSC_StringParameter(ParameterName, s.Value, CanBeEmpty);
        else return this;
    }
}
