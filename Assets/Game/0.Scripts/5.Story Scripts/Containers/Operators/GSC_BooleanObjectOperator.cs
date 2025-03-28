using UnityEngine;

[CreateAssetMenu(menuName = "Story/Boolean Operator")]
public class GSC_BooleanObjectOperator : ScriptableObject
{
    [SerializeField] private GSC_BooleanOperator Operator;
    public void Operate() => Operator.Operate();
}
