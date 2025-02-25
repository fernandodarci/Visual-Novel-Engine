using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Providers/Characters List")]
public class GSC_CharactersList : ScriptableObject
{
    [SerializeField] private List<GSC_Character> Characters;

    [Serializable]
    public class GSC_Character
    {
        public string CharacterName;
        public Color CharacterNameColor;
    }
}