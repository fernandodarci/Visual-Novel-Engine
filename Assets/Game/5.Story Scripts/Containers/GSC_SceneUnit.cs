using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_SceneUnit : MonoBehaviour
{
    [SerializeReference] public List<GSC_SceneAction> Actions;

    public void SetActionReference<T>(T action) where T : GSC_SceneAction
    {
        Actions.Add(action);
    }
}

