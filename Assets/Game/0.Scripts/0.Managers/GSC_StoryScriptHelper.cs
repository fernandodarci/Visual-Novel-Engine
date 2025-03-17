using System;
using Unity.VisualScripting;

public static class GSC_StoryScriptHelper
{
    public static void CreateStoryFromScript(this GSC_Story story)
    {
        story.Sequences = new();

        GSC_SceneSequenceUnit entryPoint = new GSC_SceneSequenceUnit();
        story.Sequences.Add(entryPoint);

        GSC_SceneUnit scene01 = new GSC_SceneUnit();
        entryPoint.Scenes.Add(scene01);

        GSC_ChangeBackgroundAction changeBackgroundScene01
            = new GSC_ChangeBackgroundAction()
            {
                Group = "One Bed Ap",
                Frame = "Nav01",
                Layer = 0,
                FadeTime = 2f,
                WaitForSeconds = 4f,
                HideAfter = false,
            };
        scene01.SetActionReference(changeBackgroundScene01);

        GSC_ShowDialogueAction dialogueScene01 = new GSC_ShowDialogueAction();
        scene01.SetActionReference(dialogueScene01);
        
        
        
        
    }

}