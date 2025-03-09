using NUnit.Framework;

public class GSC_IntegerParameter : GSC_Parameter
{
    public readonly int RangeMin;
    public readonly int RangeMax;
    public readonly int Value;

    public GSC_IntegerParameter(string parameterName, int rangeMin, int rangeMax, int value)
        : base(parameterName, GSC_ParameterType.Integer)
    {
        RangeMin = rangeMin;
        RangeMax = rangeMax;
        Value = value;
    }

    public GSC_IntegerParameter(string parameterName) 
        : this(parameterName, int.MinValue, int.MaxValue, 0) { }

    public GSC_IntegerParameter(GSC_IntegerParameter reference, int value) :
        this(reference.ParameterName, reference.RangeMin, reference.RangeMax, value) { }
    
    public static GSC_IntegerParameter WithRangeMin(string parameterName, int rangeMin)
        => new(parameterName, rangeMin, int.MaxValue, 0);

    public static GSC_IntegerParameter WithRangeMax(string parameterName, int rangeMax)
       => new(parameterName, int.MinValue, rangeMax, 0);

    public static GSC_IntegerParameter WithValue(string name, int value)
    => new(name, int.MinValue, int.MaxValue, value);

    public GSC_IntegerParameter OnValid(GSC_Parameter parameter)
    {
        if (parameter is GSC_IntegerParameter i && i.Value >= RangeMin && i.Value <= RangeMax)
                return new GSC_IntegerParameter(ParameterName, RangeMin, RangeMax, i.Value);
            else return this;
        
    }
}
