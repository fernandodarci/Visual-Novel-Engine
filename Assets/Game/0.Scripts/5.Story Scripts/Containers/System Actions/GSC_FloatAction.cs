public class GSC_FloatAction : GSC_ValueAction
{
    public enum GSC_FloatActionOperator
    {
        SET, ADD, SUBTRACT, MULTIPLY, DIVIDE, MOD, MIN, MAX
    }

    public GSC_FloatActionOperator Operator;
    public float Value;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new GSC_ContainerUnit("SetFloat");
        unit.Set("KeyToSet", KeyToSet);
        unit.Set("System", System);
        switch (Operator)
        {
            case GSC_FloatActionOperator.SET: unit.Set("OP", "SET"); break;
            case GSC_FloatActionOperator.ADD: unit.Set("OP", "ADD"); break;
            case GSC_FloatActionOperator.SUBTRACT: unit.Set("OP", "SUB"); break;
            case GSC_FloatActionOperator.MULTIPLY: unit.Set("OP", "MUL"); break;
            case GSC_FloatActionOperator.DIVIDE: unit.Set("OP", "DIV"); break;
            case GSC_FloatActionOperator.MOD: unit.Set("OP", "MOD"); break;
            case GSC_FloatActionOperator.MIN: unit.Set("OP", "MIN"); break;
            case GSC_FloatActionOperator.MAX: unit.Set("OP", "MAX"); break;
        }
        unit.Set("Value", Value);
        return unit;
    }

    public override bool Validate(GSC_ContainerUnit unit)
    {
        return unit != null && unit.Calling == "SetFloat" && unit.HasBoolean("KeyToSet")
            && unit.HasBoolean("System") && unit.HasString("OP") && unit.HasInteger("Value");
    }
}
