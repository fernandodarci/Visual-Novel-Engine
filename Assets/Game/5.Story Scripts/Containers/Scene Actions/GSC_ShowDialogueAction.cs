using UnityEngine;

public class GSC_ShowDialogueAction : GSC_SceneAction
{
    public new const string ID = "ID02";
    [SerializeField] private string CharacterName;
    [SerializeField][TextArea] private string Dialogue;
    [SerializeField] private bool Append;
    [SerializeField] private float Duration;
    [SerializeField] private float FadeTime;
    [SerializeField] private bool WaitToComplete;
    [SerializeField] private float WaitUntilComplete;
    public override string GetID() => ID;

    public override GSC_ContainerUnit Compile()
    {
        GSC_ContainerUnit unit = new("ShowDialogue");
        if (CharacterName.Contains(" as "))
        {
            int index = CharacterName.IndexOf(" as ");
            unit.Set("Character", CharacterName[..index].Trim());
            unit.Set("As", CharacterName[(index + 4)..].Trim());
        }
        else
        {
            unit.Set("Character", CharacterName.Trim());
        }
        unit.Set("Dialogue", Dialogue);
        unit.Set("Append", Append);
        unit.Set("Duration", Duration);
        unit.Set("Fade", FadeTime);
        unit.Set("WaitToComplete", WaitToComplete);
        return unit;
    }

    public override void Decompile(string json)
    {
        GSC_ContainerUnit result = GetContainer(json);
        if (result != null)
        {
            if (result.HasString("As"))
            {
                string character = result.GetString("Character");
                string alias = result.GetString("As");
                CharacterName = $"{character} as {alias}".Trim();
            }
            else CharacterName = result.GetString("Character").Trim();

            Dialogue = result.GetString("Dialogue");
            Append = result.GetBoolean("Append");
            Duration = result.GetFloat("Duration");
            FadeTime = result.GetFloat("Fade");
            WaitToComplete = result.GetBoolean("WaitToComplete");
        }
    }
}
