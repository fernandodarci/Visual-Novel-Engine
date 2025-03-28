using UnityEditor;
using UnityEngine;

public class ShowDialogueActionWizard : ScriptableWizard
{
    public string CharacterName;
    public string NickName;
    [TextArea] public string Dialogue;
    public bool Append;
    public float Duration;
    public float FadeTime;
    public bool WaitToComplete;
    public float WaitUntilComplete;
    public bool HideAfterFinished;

    [MenuItem("Tools/Show Dialogue Action Wizard")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<ShowDialogueActionWizard>("Create Show Dialogue Action", "Create");
    }

    private void OnWizardCreate()
    {
        GSC_ShowDialogueAction action = new GSC_ShowDialogueAction(
            CharacterName,
            NickName,
            Dialogue,
            Append,
            WaitToComplete,
            HideAfterFinished,
            Duration,
            FadeTime,
            WaitUntilComplete
        );

        // Here you can add the action to your story or perform other operations
        Debug.Log("Show Dialogue Action created");
    }

    private void OnWizardUpdate()
    {
        helpString = "Fill in the details for the Show Dialogue Action";
        errorString = string.IsNullOrEmpty(CharacterName) ? "Character Name is required" : "";
    }
}
