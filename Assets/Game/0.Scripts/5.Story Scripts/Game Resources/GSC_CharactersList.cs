using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Providers/Characters List")]
public class GSC_CharactersList : ScriptableObject
{
    [SerializeField] private List<GSC_Character> Characters;

    public string GetName(string Name, string Nickname)
    {
        string currentName = string.IsNullOrWhiteSpace(Nickname) ? Name : Nickname;
        
        GSC_Character character = Characters.Find(x => x.CharacterName == Name);
        if (character != null)
        {
            currentName.Trim();
            string rgb = ColorUtility.ToHtmlStringRGB(character.CharacterNameColor);
            return $"<color=#{rgb}>{currentName}</color>";
        }
        else return $"<color=black>{currentName}</color>";
    }

    public bool IsCharacter(string currentCharacter)
    {
        GSC_Character character = Characters.Find(x => x.CharacterName == currentCharacter);
        return character != null;
    }

    [Serializable]
    public class GSC_Character
    {
        public string CharacterName;
        public Color CharacterNameColor;
    }
}
