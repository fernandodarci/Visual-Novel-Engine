using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Hide Dialogue")]
public class GSC_HideDialogueActionObject : ScriptableObject
{
    [SerializeField] private GSC_HideDialogueAction action;
    public GSC_HideDialogueAction Action => action;
}
