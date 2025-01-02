using System.Collections;
using UnityEngine;

public class GSC_ChoicePanelController : GSC_ImageController
{
    [SerializeField] private GSC_ImageController Panel;
    [SerializeField] private GSC_ImageController DescriptorBackground;
    [SerializeField] private GSC_ImageController Separator;
    [SerializeField] private GSC_ScreenTextBuilder Descriptor;
    [SerializeField] private GSC_ButtonBar Choices;

    private GSC_StringParameter OptionChoosed;
    public GSC_StringParameter GetOption() => OptionChoosed;


    public void Initialize(string parameterName)
    {
        OptionChoosed = new GSC_StringParameter(parameterName);
        Descriptor.Clear();
    }

    public void SetupPanel(string[] values)
    {
        Choices.SetButtons(values);
    }

    public IEnumerator ShowDescription(string dialogue, float duration)
    {
        yield return Descriptor.BuildText(dialogue, duration, false);
    }

    public IEnumerator WaitForChoice()
    {
        yield return Choices.WaitChooseOptions();
    }
   
    
}

