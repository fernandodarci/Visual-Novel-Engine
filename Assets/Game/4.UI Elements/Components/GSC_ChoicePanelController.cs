using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_ChoicePanelController : GSC_CanvasGroupController
{
    [SerializeField] private GSC_CanvasGroupController ChoicePanel;
    [SerializeField] private GSC_CanvasGroupController MessageBackground;
    [SerializeField] private GSC_ScreenTextBuilder Message;
    [SerializeField] private Image Separator;
    [SerializeField] private GSC_ButtonBar Choices;

    private GSC_StringParameter OptionChoosed;
    public GSC_StringParameter GetOption() => OptionChoosed;
    public void Initialize(string parameterName)
    {
        OptionChoosed = new GSC_StringParameter(parameterName);
    }

    public void SetupPanel(string[] values)
    {
        Choices.SetButtons(values);
    }

    public IEnumerator WaitForChoice()
    {
        yield return Choices.WaitChooseOptions();
    }

    public IEnumerator ShowMessage(string message, float time, Func<bool> paused, Func<bool> ends)
    {
        yield return Message.BuildText(message, time, false, paused, ends);
    }
}

