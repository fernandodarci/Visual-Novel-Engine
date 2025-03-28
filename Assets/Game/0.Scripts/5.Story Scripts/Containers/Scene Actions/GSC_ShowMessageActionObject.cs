using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Show Message Action")]
public class GSC_ShowMessageActionObject : ScriptableObject
{
    [SerializeField] private GSC_ShowMessageAction action;
    public GSC_ShowMessageAction Action => action;
}