

using System;

public class GSC_FloatParameter : GSC_Parameter
{
    public readonly float RangeMin;
    public readonly float RangeMax;
    public readonly float Value;

    public GSC_FloatParameter(string parameterName, float rangeMin, float rangeMax, float value)
        : base(parameterName, GSC_ParameterType.Float)
    {
        RangeMin = rangeMin;
        RangeMax = rangeMax;
        Value = value;
    }

    public GSC_FloatParameter(string parameterName) :
        this(parameterName, float.MinValue, float.MaxValue, 0f)
    { }

    public GSC_FloatParameter(GSC_FloatParameter reference, float value) 
        : this(reference.ParameterName, reference.RangeMin, reference.RangeMax,value) { }

    public static GSC_FloatParameter WithRangeMin(string parameterName, float rangeMin)
        => new(parameterName, rangeMin, float.MaxValue, 0f);

    public static GSC_FloatParameter WithRangeMax(string parameterName, float rangeMax)
       => new(parameterName, float.MinValue, rangeMax, 0f);

    public static GSC_FloatParameter WithValue(string name, float value)
        => new(name,float.MinValue,float.MaxValue, value);

    public GSC_FloatParameter OnValid(GSC_Parameter parameter)
    {
        if (parameter is GSC_FloatParameter f && f.Value >= RangeMin && f.Value <= RangeMax)
            return new GSC_FloatParameter(ParameterName, RangeMin, RangeMax, f.Value);
        else return this;
    }
}
