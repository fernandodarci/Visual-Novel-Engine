using System;
using System.Collections;
using UnityEngine;

public class GSC_DialogueRegister : GSC_CommandRegister
{
    public new static void AddCommands(GSC_CommandDatabase database)
    {
        database.AddCommand("ShowDialogue", new Func<GSC_ContainerUnit, IEnumerator>(ShowDialogue), null)
            .With(new GSC_StringParameter("Character"))
            .With(new GSC_StringParameter("Dialogue"))
            .With(new GSC_BooleanParameter("Append"))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
            .With(new GSC_BooleanParameter("WaitToComplete"));

        database.AddCommand("ShowInput", new Func<GSC_ContainerUnit, IEnumerator>(ShowInput), null)
            .With(new GSC_StringParameter("Description"))
            .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
            .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
            .With(new GSC_BooleanParameter("System"));


        database.AddCommand("ShowChoice", new Func<GSC_ContainerUnit, IEnumerator>(ShowChoices), null)
           .With(new GSC_StringParameter("Description"))
           .With(GSC_FloatParameter.WithRangeMin("Duration", 1f))
           .With(GSC_FloatParameter.WithRangeMin("Fade", 0f))
           .With(new GSC_StringParameter("TargetResult"))
           .With(new GSC_BooleanParameter("System"));
    }

    private static IEnumerator ShowDialogue(GSC_ContainerUnit unit)
    {
        yield return Game.Dialogue.ShowDialoguePanel(unit.GetFloat("Fade"));
        Game.Dialogue.ChangeCharacterName(unit.GetString("Character"));
        string dialogue = unit.GetString("Dialogue");
        float textTime = unit.GetFloat("Duration");
        bool append = unit.GetBoolean("Append");
        yield return Game.Dialogue.ShowDialogue(dialogue, textTime, append);
        if (unit.HasBoolean("WaitToComplete"))
        {
            yield return Game.WaitForComplete();
        }
        Game.Ends();
    }

    private static IEnumerator ShowInput(GSC_ContainerUnit unit)
    {
        if (unit is GSC_ContainerUnit<GSC_Parameter> @param)
        {
            if (Game.Dialogue.IsVisible)
                yield return Game.Dialogue.HideDialogue(unit.GetFloat("Fade"));
            GSC_Parameter parameter = param.Get();
            if (parameter == null) Debug.Log("Error getting parameter");
            bool systemParam = param.GetBoolean("System");
            Game.Input.InitializeInput(parameter);
            Game.DisableScreenInput();
            yield return Game.Input.FadeIn(unit.GetFloat("Fade"));
            Game.Input.Enable(true);
            yield return Game.Input
                .ShowDescriptor(unit.GetString("Description"), unit.GetFloat("Duration"));
            yield return Game.Input.WaitForInput();
            Game.HandleInput(systemParam);
            yield return Game.Input.FadeOut(unit.GetFloat("Fade"));
            Game.Input.Disable();
            Game.EnableScreenInput();
            Game.Ends();
        }

    }

    private static IEnumerator ShowChoices(GSC_ContainerUnit unit)
    {
        if (unit is GSC_ContainerUnit<string[]> @choiceUnit)
        {
            if (Game.Dialogue.IsVisible)
                yield return Game.Dialogue.HideDialogue(choiceUnit.GetFloat("Fade"));
            Game.Choice.Initialize(choiceUnit.GetString("TargetResult"));
            Game.DisableScreenInput();
            yield return Game.Choice.FadeIn(choiceUnit.GetFloat("Fade"));
            Game.Choice.Enable(true);
            yield return Game.Choice
                .ShowDescription(choiceUnit.GetString("Description"), choiceUnit.GetFloat("Duration"));
            Game.Choice.SetupPanel(choiceUnit.Get());
            yield return Game.Choice.WaitForChoice();
            Game.HandleChoice(choiceUnit.GetBoolean("System"));
            yield return Game.Choice.FadeOut(choiceUnit.GetFloat("Fade"));
            Game.Choice.Disable();
            Game.EnableScreenInput();
            Game.Ends();
        }
    }
}