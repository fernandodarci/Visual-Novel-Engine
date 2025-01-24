using System.Collections;
using UnityEngine;

public class GSC_InputPanelController : GSC_ScreenMessageController
{
   
    [SerializeField] private GSC_ImageController Separator;
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
            yield return Input.Refresh();
        }
    }

    
}

