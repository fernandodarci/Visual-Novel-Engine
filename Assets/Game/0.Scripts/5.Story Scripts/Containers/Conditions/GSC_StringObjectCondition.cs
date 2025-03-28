using UnityEngine;

[CreateAssetMenu(menuName = "Story/String Condition")]
public class GSC_StringObjectCondition : ScriptableObject
{
    [SerializeField] private GSC_StringCondition condition;

    public bool CompareValue() => condition.CompareValue();
}