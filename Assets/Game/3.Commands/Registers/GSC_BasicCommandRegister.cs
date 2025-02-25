using System;
using System.Collections;
using UnityEngine;

public class GSC_BasicCommandRegister : GSC_CommandRegister
{
    
    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("WaitForSeconds",
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(WaitForSeconds), 
            null)
            .With(GSC_FloatParameter.WithRangeMin("Time",0f));
        database.AddCommand("WaitForInput", 
            new Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator>(WaitForInput), 
            null);
    }

    private static IEnumerator WaitForSeconds(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    {
        float seconds = unit.GetFloat("Time");
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            if (ends()) break;

            if (!paused())
            {
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
        Command.Ends();
    }

    private static IEnumerator WaitForInput(GSC_ContainerUnit unit, Func<bool> isGamePaused, Func<bool> ends)
    {
        while(!ends())
        {
            yield return null;
        }
        Command.Ends();
    }

 
}
