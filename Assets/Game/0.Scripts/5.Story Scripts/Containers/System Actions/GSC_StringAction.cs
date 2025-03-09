public class GSC_StringAction : GSC_ValueAction
{
public string Value;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("SetString");
        unit.Set("KeyToSet", KeyToSet);
        unit.Set("System", System);
        unit.Set("Value", Value);
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "SetBoolean" && unit.HasBoolean("KeyToSet")
            && unit.HasBoolean("System") && (unit.HasBoolean("Value") || unit.HasBoolean("ChangeValue"));
    }
}
