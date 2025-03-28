using UnityEngine;

[CreateAssetMenu(menuName = "Story/Boolean Condition")]
public class GSC_BooleanObjectCondition : ScriptableObject
{
    [SerializeField] private GSC_BooleanCondition condition;

    public bool CompareValue() => condition.CompareValue();
}
