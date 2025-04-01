public static class GSC_ScriptUnitExtensions
{
    public static void AddOrCompleteDialogueAction(this GSC_ScriptUnit unit, GSC_ShowDialogueBoxAction action)
    {
        if (unit.Actions.IsNullOrEmpty())
        {
            unit.Actions = new(new[] { action });
            return;
        }
        else
        {
            if (unit.Actions[^1] is GSC_ShowDialogueBoxAction @act && act.IsHead() && action.IsTail())
            {
                action.Character = act.Character;
                action.NameToShow = act.NameToShow;
                unit.Actions[^1] = action;
                return;
            }
            else unit.Actions.Add(action);
        }
    }
}