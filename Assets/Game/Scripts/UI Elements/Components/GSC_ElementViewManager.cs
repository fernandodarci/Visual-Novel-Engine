using System.Collections.Generic;
using UnityEngine;

public class GSC_ElementViewManager : GSC_Singleton<GSC_ElementViewManager>
{
    [SerializeField] private List<GSC_ElementView> Elements;

    public GSC_DialogueBox GetDialogueBox()
    {
        foreach (GSC_ElementView element in Elements)
        {
            if (element is GSC_DialogueBox dialogueBox)
                return dialogueBox;
        }
        return null;
    }
}

