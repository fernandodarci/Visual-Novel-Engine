using System.Collections.Generic;

public static class GSC_ContainerUnitValidatorExtensions
{
    private static bool ValidateParameter(this GSC_ContainerUnit unit, string name, GSC_ParameterType type)
    {
        return type switch
        {
            GSC_ParameterType.Boolean => unit.HasBoolean(name),
            
            GSC_ParameterType.Zero_or_Positive_Integer 
            => unit.HasInteger(name) && unit.GetInteger(name) >= 0,
            
            GSC_ParameterType.Zero_or_Negative_Integer 
            => unit.HasInteger(name) && unit.GetInteger(name) <= 0,
            
            GSC_ParameterType.NonZero_Positive_Integer 
            => unit.HasInteger(name) && unit.GetInteger(name) > 0,
            
            GSC_ParameterType.NonZero_Negative_Integer 
            => unit.HasInteger(name) && unit.GetInteger(name) < 0,
            
            GSC_ParameterType.Integer => unit.HasInteger(name),
            
            GSC_ParameterType.Zero_or_Positive_Float 
            => unit.HasFloat(name) && unit.GetFloat(name) >= 0f,
            
            GSC_ParameterType.Zero_or_Negative_Float 
            => unit.HasFloat(name) && unit.GetFloat(name) <= 0f,
            
            GSC_ParameterType.NonZero_Positive_Float 
            => unit.HasFloat(name) && unit.GetFloat(name) > 0f,
            
            GSC_ParameterType.NonZero_Negative_Float 
            => unit.HasFloat(name) && unit.GetFloat(name) < 0f,
            
            GSC_ParameterType.Float => unit.HasFloat(name),
            
            GSC_ParameterType.NonEmpty_String 
            => unit.HasString(name) && !string.IsNullOrWhiteSpace(unit.GetString(name)),
            
            GSC_ParameterType.String => unit.HasString(name),
            
            _ => false //if name is not a GSC_ContainerUnit parameter.
        };
    }

    public static bool Validate(this GSC_ContainerUnit unit, 
        string Calling, Dictionary<string,GSC_ParameterType> TypeArgs)
    {
        //If unit is null, or the calling of it is different that one we are waiting for, it's invalid.
        if (unit == null || string.IsNullOrWhiteSpace(unit.Calling) || unit.Calling != Calling)
            return false;

        //if no validators, no need to check anything else.
        if (TypeArgs == null || TypeArgs.Count == 0) return true; 

        foreach (var parameter in TypeArgs)
        {
            if (!unit.ValidateParameter(parameter.Key, parameter.Value))
                return false;
        }
        return true; //All parameters are valid
    }

    public static bool Validate<T>(this GSC_ContainerUnit<T> unit, 
        string Calling, Dictionary<string,GSC_ParameterType> TypeArgs)
    {
        return (unit as GSC_ContainerUnit).Validate(Calling, TypeArgs) && unit.HasAsset();
    }
}
