using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/NavPoint Action")]
public class GSC_NavPointActionObject : ScriptableObject
{
    [SerializeField] private GSC_NavPointAction action;
    public GSC_NavPointAction Action => action;
}
