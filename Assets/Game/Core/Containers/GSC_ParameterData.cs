using System.Collections.Generic;

public class GSC_ParameterData
{
    private readonly List<GSC_Parameter> Parameters;

    public GSC_ParameterData()
    {
        Parameters = new List<GSC_Parameter>();
    }

    public void Set(GSC_Parameter parameter)
    {
        GSC_Parameter param = Get(parameter.ParameterName);
        if (param != null)
        {
            Parameters.Remove(param);
            switch(param)
            {
                case GSC_BooleanParameter b: Parameters.Add(b.OnValid(parameter)); break;
                case GSC_IntegerParameter i: Parameters.Add(i.OnValid(parameter)); break;
                case GSC_FloatParameter f: Parameters.Add(f.OnValid(parameter)); break;
                case GSC_StringParameter s: Parameters.Add(s.OnValid(parameter)); break;
            };
        }
        else Parameters.Add(parameter);
    }

    public GSC_Parameter Get(string key)
    => Parameters.Find(x => x.ParameterName == key);
    
    public string GetAsString(string key, string OnFalse = "False",string OnTrue = "True")
    {
        GSC_Parameter parameter = Get(key);
        if (parameter == null) return key;
        return parameter switch
        {
            GSC_BooleanParameter b => b.Value == true ? OnTrue : OnFalse,
            GSC_IntegerParameter i => i.Value.ToString(),
            GSC_FloatParameter f => f.Value.ToString(),
            GSC_StringParameter s => s.Value,
            _ => key,
        };
    }
}