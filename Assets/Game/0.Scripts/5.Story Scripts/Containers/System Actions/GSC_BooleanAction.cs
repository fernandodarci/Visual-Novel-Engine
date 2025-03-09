public class GSC_BooleanAction : GSC_ValueAction
{
    public enum GSC_BooleanActionOperator
    {
        SET_TO_TRUE, SET_TO_FALSE, CHANGE
    }

    public GSC_BooleanActionOperator Operator;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("SetBoolean");
        unit.Set("KeyToSet", KeyToSet);
        unit.Set("System", System);
        switch (Operator)
        {
            case GSC_BooleanActionOperator.SET_TO_TRUE: unit.Set("Value", true); break;
            case GSC_BooleanActionOperator.SET_TO_FALSE: unit.Set("Value", false); break;
            case GSC_BooleanActionOperator.CHANGE: unit.Set("ChangeValue", true); break;
        }
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "SetBoolean" && unit.HasBoolean("KeyToSet")
            && unit.HasBoolean("System") && (unit.HasBoolean("Value") || unit.HasBoolean("ChangeValue"));
    }
}
