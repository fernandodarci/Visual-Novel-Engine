using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GSC_OptionsPanelController : GSC_CanvasGroupController
{
    [SerializeField] private GSC_CanvasGroupController OptionsPanel;
    [SerializeField] private GSC_CanvasGroupController MessageBackground;
    [SerializeField] private GSC_ScreenTextBuilder Message;
    [SerializeField] private Image Separator;
    [SerializeField] private GSC_ButtonBar Options;

    public string GetOption() => Options.OptionChoosed;
    public void ClearMessage() => Message.Clear();
    public void SetupPanel(string[] values) => Options.SetButtons(values);
    public IEnumerator WaitForChoice()
    {
        yield return Options.WaitChooseOptions();
    }

    public IEnumerator ShowMessage(string message, float time, Func<bool> paused, Func<bool> ends)
    {
        yield return Message.BuildText(message, time, false, paused, ends);
    }
}

