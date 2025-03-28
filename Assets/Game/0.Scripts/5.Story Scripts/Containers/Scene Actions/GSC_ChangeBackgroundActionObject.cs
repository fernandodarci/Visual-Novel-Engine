using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Change Background")]
public class GSC_ChangeBackgroundActionObject : ScriptableObject
{
    [SerializeField] private GSC_ChangeBackgroundAction action;

    public GSC_ChangeBackgroundAction Action => action;
}
