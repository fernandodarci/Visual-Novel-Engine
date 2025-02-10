using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_GraphicsManager : GSC_Singleton<GSC_GraphicsManager>
{
    [Serializable]
    public class GSC_ImageLayers
    {
        public string LayerName;
        public GSC_ImageLayerController Controller;
    }

    [SerializeField] private List<GSC_ImageLayers> Controllers;
}

