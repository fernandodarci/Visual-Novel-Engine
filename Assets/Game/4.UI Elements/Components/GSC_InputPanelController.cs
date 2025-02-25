using System;
using System.Collections;
using UnityEngine;

public class GSC_InputPanelController : GSC_CanvasGroupController
{
    [SerializeField] private GSC_CanvasGroupController InputPanel;
    [SerializeField] private GSC_CanvasGroupController InputBackground;
    [SerializeField] private GSC_ScreenTextBuilder Message;
    [SerializeField] private GSC_CanvasGroupController Separator;
    [SerializeField] private GSC_InputHandler Input;

    public GSC_Parameter Parameter => Input.InputData;

    public void InitializeInput(GSC_Parameter parameter)
    {
        Input.InitializeInput(parameter);
        Input.gameObject.SetActive(false);
    }

    
    public IEnumerator WaitForInput()
    {
        Input.gameObject.SetActive(true);
        while (Input.IsInput == false)
        {
            yield return Input.WaitingForInput();
        }
    }

    public IEnumerator ShowMessage(string message, float messageTime, Func<bool> paused, Func<bool> ends)
    {
        if (IsVisible)
        {
            yield return Message.BuildText(message, messageTime, false, paused, ends);
        }
    }
}

