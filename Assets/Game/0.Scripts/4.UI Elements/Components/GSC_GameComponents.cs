
using System;
using System.Collections.Generic;
using UnityEngine;


public class GSC_GameComponents : MonoBehaviour
{
    [Serializable]
    private class GSC_ScreenMessageComponent
    {
        public string Name;
        public GSC_ScreenMessageController Controller;
    }

     [Header("Screen Message Controllers")]
    [SerializeField] private List<GSC_ScreenMessageComponent> ScreenMessages;
    
   
    
}

