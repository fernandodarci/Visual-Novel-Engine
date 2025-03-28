using UnityEngine;

[CreateAssetMenu(menuName = "Story/Integer Operator")]
public class GSC_IntegerObjectOperator : ScriptableObject
{
    [SerializeField] private GSC_IntegerOperator Operator;
    public void Operate() => Operator.Operate();
}
