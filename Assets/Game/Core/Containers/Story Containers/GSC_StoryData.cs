using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Story Data")]
public class GSC_StoryData : ScriptableObject
{
    [SerializeField] private string StoryName;
    [SerializeField] private GSC_CharacterProvider Characters;
    [SerializeField] private GSC_ProviderContainer Assets;
}
