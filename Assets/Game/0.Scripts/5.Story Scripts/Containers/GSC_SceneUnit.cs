using System;
using System.Collections.Generic;
using UnityEngine;

public class GSC_SceneUnit : MonoBehaviour
{
    [SerializeReference] public List<GSC_Action> Actions;

    public void SetActionReference<T>(T action) where T : GSC_Action
    {
        Actions.Add(action);
    }
}

