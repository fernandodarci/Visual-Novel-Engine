using UnityEngine;

public abstract class GSC_ValueComparer : MonoBehaviour
{
    public string KeyToCompare;
    public bool System;

    public abstract bool Compare();
}
