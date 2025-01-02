using System;
using System.Collections;
using UnityEngine;

public class GSC_BasicCommandRegister : GSC_CommandRegister
{
    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("WaitForSeconds", new Func<GSC_ContainerUnit,IEnumerator>(WaitForSeconds), null)
            .With(GSC_FloatParameter.WithRangeMin("Time",0f));
        database.AddCommand("WaitForInput", new Func<GSC_ContainerUnit, IEnumerator>(WaitForInput), null);
    }

    private static IEnumerator WaitForSeconds(GSC_ContainerUnit unit)
    {
        float seconds = unit.GetFloat("Time");
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            if (Game.RequestToTerminate == true)
            {
                yield break;
            }
            else if (Game.IsExecuting)
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
            Game.Ends();
        }
    }

    private static IEnumerator WaitForInput(GSC_ContainerUnit unit)
    {
        Game.RequestInput();
        while(Game.InputRequested == false)
        {
            yield return null;
        }
        Game.Ends();
    }
}
