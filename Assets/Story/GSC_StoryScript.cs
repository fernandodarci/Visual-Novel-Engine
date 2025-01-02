using UnityEngine;

public static class GSC_StoryScript
{
    public static GSC_StoryUnit GetScript()
    {
        return new GSC_StoryFactory()
            .StartStory()
            .WithChapter("1","1")
            .WithScene(0)
            .WithContainer("ShowDialogue")
            .WithParameter("Character","System")
            .WithParameter("Dialogue","Hey, you. Welcome to our game. Ready for breathtaking?")
            .WithParameter("Append",false)
            .WithParameter("Duration",2f)
            .WithParameter("Fade",1f)
            .WithParameter("WaitToComplete",true)
            .WithScene(1)
            .WithContainer("ShowInput", new GSC_StringParameter("Name","Aldo",false))
            .WithParameter("Description", "To start, please enter your name")
            .WithParameter("Duration", 2f)
            .WithParameter("Fade", 1f)
            .WithParameter("System",true)
            .WithScene(2)
            .WithContainer("ShowChoice", new string[] { "Peace", "Revenge", "Justice" })
            .WithParameter("Description", "Choose your path of life.")
            .WithParameter("Duration", 2f)
            .WithParameter("Fade", 1f)
            .WithParameter("TargetResult","Starting Choice")
            .WithParameter("System",true)
            .End().MainStory;
    
    }
}
