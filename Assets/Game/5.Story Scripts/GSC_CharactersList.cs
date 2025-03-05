using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Providers/Characters List")]
public class GSC_CharactersList : ScriptableObject
{
    [SerializeField] private List<GSC_Character> Characters;

    public string GetName(string Name, string As)
    {
        string currentName = string.IsNullOrWhiteSpace(As) ? Name : As;
        GSC_Character character = Characters.Find(x => x.CharacterName == Name);
        if (character != null)
        {
            string rgb = ColorUtility.ToHtmlStringRGB(character.CharacterNameColor);
            return $"<color=#{rgb}>{currentName}</color>";
        }
        else return $"<color=black>{currentName}</color>";
    }

    [Serializable]
    public class GSC_Character
    {
        public string CharacterName;
        public Color CharacterNameColor;
    }
}