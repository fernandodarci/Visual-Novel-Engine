using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Show Dialogue")]
public class GSC_ShowDialogueActionObject : ScriptableObject
{
    [SerializeField] private GSC_ShowDialogueAction action;

    public GSC_ShowDialogueActionObject()
    {
    }

    public GSC_ShowDialogueAction Action => action;
}
