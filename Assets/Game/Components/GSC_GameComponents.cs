
using System;
using System.Collections.Generic;
using UnityEngine;

public enum GSC_ImageControllers
{
    BACKGROUND, CHARACTERS, FOREGROUND
}

public class GSC_GameComponents : MonoBehaviour
{
    [Serializable]
    private class GSC_ScreenMessageComponent
    {
        public string Name;
        public GSC_ScreenMessageController Controller;
    }

    [SerializeField] private GSC_DialogueController DialogueController;
    [SerializeField] private GSC_InputPanelController InputPanelController;
    [SerializeField] private GSC_ChoicePanelController ChoicePanelController;
    [SerializeField] private GSC_ImageLayerController BackgroundController;
    [SerializeField] private GSC_ImageLayerController CharactersController;
    [SerializeField] private GSC_ImageLayerController ForegroundController;
    [Header("Screen Message Controllers")]
    [SerializeField] private List<GSC_ScreenMessageComponent> ScreenMessages;
    
    public GSC_DialogueController Dialogue => DialogueController;
    public GSC_InputPanelController InputPanel => InputPanelController;
    public GSC_ChoicePanelController ChoicePanel => ChoicePanelController;
    public GSC_ImageLayerController GetController(GSC_ImageControllers controller)
    {
        return controller switch
        {
            GSC_ImageControllers.BACKGROUND => BackgroundController,
            GSC_ImageControllers.CHARACTERS => CharactersController,
            GSC_ImageControllers.FOREGROUND => ForegroundController,
            _ => null,
        };
    }
    
    public GSC_ScreenMessageController GetController(string name)
    {
        GSC_ScreenMessageComponent scr = ScreenMessages.Find(x => x.Name == name);
        if (scr == null) return null;
        else
        {
            foreach(var msg in ScreenMessages)
            {
                if (msg.Name != scr.Name)
                {
                    msg.Controller.Disable();
                }
            }
            return scr.Controller;
        }
    }

    public void DisableAll()
    {
        InputPanel.Disable();
        DialogueController.Disable();
        ChoicePanel.Disable();
        if(ScreenMessages.Count > 0)
        {
            foreach(var msg in ScreenMessages) msg.Controller.Disable();
        }
    }
}

