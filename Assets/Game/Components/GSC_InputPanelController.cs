using System;
using System.Collections;
using UnityEditor.Search;
using UnityEngine;

public class GSC_InputPanelController : GSC_ImageController
{
    [SerializeField] private GSC_ImageController Panel;
    [SerializeField] private GSC_ImageController DescriptorBackground;
    [SerializeField] private GSC_ImageController Separator;
    [SerializeField] private GSC_ScreenTextBuilder Descriptor;
    [SerializeField] private GSC_InputHandler Input;

    public GSC_Parameter Parameter => Input.InputData;

    public void InitializeInput(GSC_Parameter parameter)
    {
        Input.InitializeInput(parameter);
        Descriptor.Clear();
        Input.gameObject.SetActive(false);
    }

    public IEnumerator ShowDescriptor(string text, float duration)
    {
        yield return Descriptor.BuildText(text,duration,false);
    }

    public IEnumerator WaitForInput()
    {
        Input.gameObject.SetActive(true);
        while (Input.IsInput == false)
        {
            yield return Input.Refresh();
        }
    }

    
}

