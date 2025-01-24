public abstract class GSC_CommandRegister
{
    protected static GSC_GameManager Game => GSC_GameManager.Instance;
    protected static GSC_ScriptManager Script => GSC_ScriptManager.Instance;
    public static void AddCommands(GSC_CommandDatabase database) { }
}
