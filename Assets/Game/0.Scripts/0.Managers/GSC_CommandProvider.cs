using System;
using System.Collections;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UIElements;

public class GSC_CommandProvider : GSC_Singleton<GSC_CommandProvider>
{
    private static GSC_DialogueManager Dialogues => GSC_DialogueManager.Instance;

   

  

    
 


    //private static IEnumerator ShowInput(GSC_ContainerUnit unit, Func<bool> paused, Func<bool> ends)
    //{

    //    if (unit is GSC_ContainerUnit<GSC_Parameter> @param)
    //    {
    //        GSC_Parameter parameter = param.Get();
    //        if (parameter == null) Debug.Log("Error getting parameter");
    //        bool systemParam = param.GetBoolean("System");
    //        var inputController = Dialogues.GetInput();
    //        if (inputController != null)
    //        {
    //            inputController.InitializeInput(parameter);
    //            yield return inputController.FadeIn(unit.GetFloat("Fade"), paused, ends);
    //            inputController.Enable();
    //            string message = Data.ProcessString(unit.GetString("Message"));
    //            yield return inputController
    //                .ShowMessage(message, unit.GetFloat("Duration"), paused, ends);
    //            yield return inputController.WaitForInput();
    //            yield return inputController.FadeOut(unit.GetFloat("Fade"), paused, ends);
    //            inputController.Hide();
    //        }
    //        Command.Ends();
    //    }

    //}

}