using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Story Data")]
public class GSC_StoryData : ScriptableObject
{
    [SerializeField] private string StoryName;
    [SerializeField] private GSC_CharactersList Characters;
    [SerializeField] private GSC_GameAssets Assets;

    public Sprite GetSprite(string group, string name)
        => Assets.GetSprite(group, name);
}
