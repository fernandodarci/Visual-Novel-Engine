using System.Collections.Generic;
using UnityEngine;

public class GSC_CharacterManager : GSC_Singleton<GSC_CharacterManager>
{
    [SerializeField] private List<GSC_Character> Characters;

    public string GetCharacterName(string characterName, string nameToShow)
    {
        Color color = Color.black;
        if(!characterName.IsNullOrEmpty() && !Characters.IsNullOrEmpty())
        {
            GSC_Character character = 
                Characters.Find(c => c.Name.CompareInv(characterName));
            if (character != null) color = character.Color;
        }
        string rgb = ColorUtility.ToHtmlStringRGB(color);
        return nameToShow.IsNullOrEmpty() ? $"<color=#{rgb}><b>{characterName}</b></color>" :
            $"<color=#{rgb}><b>{nameToShow}</b></color>";
    }
}

