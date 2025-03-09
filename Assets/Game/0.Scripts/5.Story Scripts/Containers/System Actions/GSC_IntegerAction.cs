public class GSC_IntegerAction : GSC_ValueAction
{
    public enum GSC_IntegerActionOperator
    {
        SET, ADD, SUBTRACT, MULTIPLY, DIVIDE, MOD, MIN, MAX
    }

    public GSC_IntegerActionOperator Operator;
    public int Value;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("SetBoolean");
        unit.Set("KeyToSet", KeyToSet);
        unit.Set("System", System);
        switch (Operator)
        {
            case GSC_IntegerActionOperator.SET: unit.Set("OP", "SET"); break;
            case GSC_IntegerActionOperator.ADD: unit.Set("OP", "ADD"); break;
            case GSC_IntegerActionOperator.SUBTRACT: unit.Set("OP", "SUB"); break;
            case GSC_IntegerActionOperator.MULTIPLY: unit.Set("OP", "MUL"); break;
            case GSC_IntegerActionOperator.DIVIDE: unit.Set("OP", "DIV"); break;
            case GSC_IntegerActionOperator.MOD: unit.Set("OP", "MOD"); break;
            case GSC_IntegerActionOperator.MIN: unit.Set("OP", "MIN"); break;
            case GSC_IntegerActionOperator.MAX: unit.Set("OP", "MAX"); break;
        }
        unit.Set("Value", Value);
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "SetInteger" && unit.HasBoolean("KeyToSet")
            && unit.HasBoolean("System") && unit.HasString("OP") && unit.HasInteger("Value");
    }
}
