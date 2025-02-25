using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_DialogueManager : GSC_Singleton<GSC_DialogueManager>
{
    [Header("Dialogue Controllers")]
    [SerializeField] private GSC_DialoguePanelController DialogueController;
    [SerializeField] private GSC_InputPanelController InputPanelController;
    [SerializeField] private GSC_ChoicePanelController ChoicePanelController;
    [SerializeField] private List<GSC_ScreenMessageController> ScreenMessages;

    private GSC_ScreenMessageController CurrentController;

    public GSC_ScreenMessageController GetMessageController(string name)
    {
        var controller = ScreenMessages.Find(x => x.ID == name);
        if(controller != null) CurrentController = controller;
        return CurrentController;
    }
  
    public void HideAllMessages()
    {
        InputPanelController.Hide();
        DialogueController.Hide();
        ChoicePanelController.Hide();
        if (ScreenMessages.Count > 0)
        {
            foreach (var msg in ScreenMessages) msg.Hide();
        }
    }

    public GSC_DialoguePanelController GetDialogue() => DialogueController;
    public GSC_InputPanelController GetInput() => InputPanelController;
    public GSC_ChoicePanelController GetChoice() => ChoicePanelController;
    
}
