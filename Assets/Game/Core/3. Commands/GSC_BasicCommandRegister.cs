using System;
using System.Collections;

public class GSC_BasicCommandRegister : GSC_CommandRegister
{
    
    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("WaitForSeconds", new Func<GSC_ContainerUnit,IEnumerator>(WaitForSeconds), null)
            .With(GSC_FloatParameter.WithRangeMin("Time",0f));
        database.AddCommand("WaitForInput", new Func<GSC_ContainerUnit, IEnumerator>(WaitForInput), null);
        database.AddCommand("ShowMainMenu", new Func<GSC_ContainerUnit, IEnumerator>(ShowMainMenu), null)
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0.5f));
    }

    private static IEnumerator WaitForSeconds(GSC_ContainerUnit unit)
    {
        float seconds = unit.GetFloat("Time");
        yield return Game.WaitForSeconds(seconds);
        Script.Ends();
        
    }

    private static IEnumerator WaitForInput(GSC_ContainerUnit unit)
    {
        Game.RequestInput();
        while(Game.InputRequested == false)
        {
            yield return null;
        }
        Script.Ends();
    }

    private static IEnumerator ShowMainMenu(GSC_ContainerUnit unit)
    {
        Game.DisableScreenInput();
        yield return Game.ShowMainMenu(unit.GetFloat("Fade"));
        Script.Ends();
    }
}
