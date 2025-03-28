using UnityEngine;

[CreateAssetMenu(menuName = "Story/Integer Condition")]
public class GSC_IntegerObjectCondition : ScriptableObject
{
    [SerializeField] private GSC_IntegerCondition condition;

    public bool CompareValue() => condition.CompareValue();
}
