using System.Collections;
using UnityEngine;

public class GSC_ChoicePanelController : GSC_ScreenMessageController
{
    [SerializeField] private GSC_ImageController Separator;
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
   
    
}

