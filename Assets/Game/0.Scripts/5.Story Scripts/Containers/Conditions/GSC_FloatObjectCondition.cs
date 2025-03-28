using UnityEngine;

[CreateAssetMenu(menuName = "Story/Float Condition")]
public class GSC_FloatObjectCondition : ScriptableObject
{
    [SerializeField] private GSC_FloatCondition condition;

    public bool CompareValue() => condition.CompareValue();
}
