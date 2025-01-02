using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Providers/Character Provider")]
public class GSC_CharacterProvider : ScriptableObject
{
    [SerializeField] private List<GSC_Character> Characters;

    [Serializable]
    public class GSC_Character
    {
        public string CharacterName;
    }
}