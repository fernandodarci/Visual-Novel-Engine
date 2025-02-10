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
       
        database.AddCommand("ShowMessage", new Func<GSC_ContainerUnit, IEnumerator>(ShowMessage), null)
           .With(new GSC_StringParameter("MessageType"))
           .With(new GSC_StringParameter("Message"))
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

    private static IEnumerator ShowMessage(GSC_ContainerUnit unit)
    {
        GSC_ScreenMessageController message = Game.GetMessageController(unit.GetString("MessageType"));
        if (message != null)
        {
            Game.EnableScreenInput();
            message.ClearText();
            string dialogue = unit.GetString("Message");
            float textTime = unit.GetFloat("Duration");
            bool append = unit.GetBoolean("Append");
            yield return message.ShowMessagePanel(unit.GetFloat("Fade"));
            yield return message.ShowMessage(dialogue, textTime, append);
            if (unit.GetBoolean("WaitToComplete") == true)
            {
                yield return Game.WaitForComplete();
            }
            yield return message.HideDialogue(unit.GetFloat("Fade"));
        }
        Script.Ends();
    }


    private static IEnumerator ShowDialogue(GSC_ContainerUnit unit)
    {
        GSC_ScreenMessageController controller = Game.GetMessageController("Dialogue");
        if (controller != null && controller is GSC_DialogueController dialogueController)
        {
            dialogueController.ClearText();
            if (unit is GSC_ContainerUnit<Color> colorUnit)
                dialogueController.ChangeCharacterNameColor(colorUnit.Get());
            dialogueController.ChangeCharacterName(unit.GetString("Character"));

            Game.EnableScreenInput();
            yield return dialogueController.ShowDialoguePanel(unit.GetFloat("Fade"));
            string dialogue = unit.GetString("Dialogue");
            float textTime = unit.GetFloat("Duration");
            bool append = unit.GetBoolean("Append");
            yield return dialogueController.ShowMessage(dialogue, textTime, append);
            if (unit.GetBoolean("WaitToComplete") == true)
            {
                yield return Game.WaitForComplete();
                dialogueController.ClearText();
                yield return dialogueController.FadeOut(unit.GetFloat("Fade"));
            }
        }
        Script.Ends();
    }
    
    
    private static IEnumerator ShowInput(GSC_ContainerUnit unit)
    {
        if (unit is GSC_ContainerUnit<GSC_Parameter> @param)
        {
            GSC_Parameter parameter = param.Get();
            if (parameter == null) Debug.Log("Error getting parameter");
            bool systemParam = param.GetBoolean("System");
            GSC_ScreenMessageController message = Game.GetMessageController("Input");
            if (message != null && message is GSC_InputPanelController @input)
            {
                input.InitializeInput(parameter);
                Game.DisableScreenInput();
                yield return input.FadeIn(unit.GetFloat("Fade"));
                input.Enable();
                yield return input
                    .ShowMessage(unit.GetString("Description"), unit.GetFloat("Duration"));
                yield return input.WaitForInput();
                Game.HandleInput(systemParam);
                yield return input.FadeOut(unit.GetFloat("Fade"));
                input.Hide();
                Game.EnableScreenInput();
            }
            Script.Ends();
        }

    }

    private static IEnumerator ShowChoices(GSC_ContainerUnit unit)
    {
        if (unit is GSC_ContainerUnit<string[]> @choiceUnit)
        {
            GSC_ScreenMessageController message = Game.GetMessageController("Choice");
            if (message != null && message is GSC_ChoicePanelController @choice)
            {
                choice.Initialize(choiceUnit.GetString("TargetResult"));
                Game.DisableScreenInput();
                yield return choice.FadeIn(choiceUnit.GetFloat("Fade"));
                choice.Enable();
                yield return choice
                    .ShowMessage(choiceUnit.GetString("Description"), choiceUnit.GetFloat("Duration"));
                choice.SetupPanel(choiceUnit.Get());
                yield return choice.WaitForChoice();
                Game.HandleChoice(choiceUnit.GetBoolean("System"));
                yield return choice.FadeOut(choiceUnit.GetFloat("Fade"));
                choice.Hide();
                Game.EnableScreenInput();
            }
            Script.Ends();
        }
    }
}