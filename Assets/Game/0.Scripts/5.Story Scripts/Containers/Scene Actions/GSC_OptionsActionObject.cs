using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Options Action")]
public class GSC_OptionsActionObject : ScriptableObject
{
    [SerializeField] private GSC_OptionsAction action;
    public GSC_OptionsAction Action => action;
}
