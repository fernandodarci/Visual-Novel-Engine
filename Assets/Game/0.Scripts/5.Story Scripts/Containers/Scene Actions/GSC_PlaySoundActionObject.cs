using UnityEngine;

[CreateAssetMenu(menuName = "Story/Scene Actions/Play Sound Action")]
public class GSC_PlaySoundActionObject : ScriptableObject
{
    [SerializeField] private GSC_PlaySoundAction action;
    public GSC_PlaySoundAction Action => action;
}
