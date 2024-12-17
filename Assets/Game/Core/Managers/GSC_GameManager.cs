using System;
using System.Collections;
using UnityEngine;

public class GSC_GameManager : GSC_Singleton<GSC_GameManager>
{
    public static GSC_CommandManager Commands => GSC_CommandManager.Instance;

    [SerializeField] private GSC_ProviderContainer GameAssets;
    [Header("Test - Remove in production")]
    [SerializeField] private GSC_DialogueController dialogueController;
    private GSC_CommandDatabase database;
    GSC_ContainerUnit Test = new GSC_ContainerUnit("DebugMessage");

    private void Start()
    {
        StartCoroutine(RunOnce());
    }

    private IEnumerator RunOnce()
    {
        yield return new WaitForEndOfFrame();
        Commands.Execute(Test);
        GSC_ContainerUnit unit = new GSC_ContainerUnit("ShowDialogue");
        unit.Set("Append", false);
        unit.Set("Character", "Test Character");
        unit.Set("Dialogue", "This is a simple line of dialogue for tests with the new system.");
        unit.Set("Duration", 2f);
        while (Commands.IsExecuting) yield return null;
        yield return dialogueController.ShowDialogue(unit);
        
        while(true)
        {

            yield return null;
        }
    }
    
}
